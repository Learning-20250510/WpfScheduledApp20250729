using Microsoft.EntityFrameworkCore;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.Services
{
    internal abstract class BaseService<T> where T : BaseEntity
    {
        protected readonly BaseDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseService(BaseDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // Common CRUD operations
        public virtual async Task<T> AddAsync(T entity)
        {
            entity.LastUpdMethodName = "Add";
            
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public virtual async Task<T?> GetByIdAndTouchAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                await TouchAsync(id);
            }
            return entity;
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<List<T>> GetAllAndTouchAsync()
        {
            var entities = await _dbSet.ToListAsync();
            if (entities.Any())
            {
                var ids = entities.Select(e => EF.Property<int>(e, "Id")).ToList();
                await TouchMultipleAsync(ids);
            }
            return entities;
        }

        public virtual async Task<T?> UpdateAsync(int id, T updatedEntity)
        {
            var existingEntity = await _dbSet.FindAsync(id);
            if (existingEntity == null) return null;

            // Update common BaseEntity properties
            existingEntity.LastUpdMethodName = "Update";
            existingEntity.ErrorMessage = updatedEntity.ErrorMessage;

            // Call virtual method to update specific properties
            UpdateEntityProperties(existingEntity, updatedEntity);

            await _context.SaveChangesAsync();
            return existingEntity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e => EF.Property<int>(e, "Id") == id);
        }

        // Archive/Disable operations (soft delete alternatives)
        public virtual async Task<bool> ArchiveAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            entity.Archived = true;
            entity.LastUpdMethodName = "Archive";

            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> DisableAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            entity.Disabled = true;
            entity.LastUpdMethodName = "Disable";

            await _context.SaveChangesAsync();
            return true;
        }

        // Get active (non-archived, non-disabled) entities
        public virtual async Task<List<T>> GetActiveAsync()
        {
            return await _dbSet
                .Where(e => !EF.Property<bool>(e, "Archived") && !EF.Property<bool>(e, "Disabled"))
                .ToListAsync();
        }

        // Abstract method for derived classes to implement specific property updates
        protected abstract void UpdateEntityProperties(T existingEntity, T updatedEntity);

        // Touch operations - update TouchedAt without modifying other fields
        public virtual async Task TouchAsync(int id)
        {
            var tableName = _context.Model.FindEntityType(typeof(T))?.GetTableName();
            if (tableName != null)
            {
                await _context.Database.ExecuteSqlAsync(
                    $"UPDATE \"{tableName}\" SET touched_at = {DateTime.UtcNow}, last_touched_method_name = {"Touch"} WHERE id = {id}");
            }
        }

        public virtual async Task TouchMultipleAsync(List<int> ids)
        {
            if (!ids.Any()) return;

            var tableName = _context.Model.FindEntityType(typeof(T))?.GetTableName();
            if (tableName != null)
            {
                await _context.Database.ExecuteSqlAsync(
                    $"UPDATE \"{tableName}\" SET touched_at = {DateTime.UtcNow}, last_touched_method_name = {"TouchMultiple"} WHERE id = ANY({ids.ToArray()})");
            }
        }

        // Transaction helper methods
        public virtual async Task<bool> AddMultipleAsync(List<T> entities)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var entity in entities)
                {
                    entity.LastUpdMethodName = "AddMultiple";
                    _dbSet.Add(entity);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}