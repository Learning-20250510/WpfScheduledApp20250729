using Microsoft.EntityFrameworkCore;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Models.Entities;
using WpfScheduledApp20250729.Utils;

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
            try
            {
                Logger.LogWithContext($"Adding entity of type {typeof(T).Name}", Logger.LogLevel.Info);
                entity.LastUpdMethodName = "Add";
                
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
                
                var entityId = _context.Entry(entity).Property("Id").CurrentValue;
                Logger.LogWithContext($"Successfully added entity with ID: {entityId}", Logger.LogLevel.Info);
                return entity;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
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
                var ids = entities.Select(e => _context.Entry(e).Property("Id").CurrentValue).Cast<int>().ToList();
                await TouchMultipleAsync(ids);
            }
            return entities;
        }

        public virtual async Task<T?> UpdateAsync(int id, T updatedEntity)
        {
            try
            {
                Logger.LogWithContext($"Updating entity of type {typeof(T).Name} with ID: {id}", Logger.LogLevel.Info);
                var existingEntity = await _dbSet.FindAsync(id);
                if (existingEntity == null) 
                {
                    Logger.LogWithContext($"Entity with ID {id} not found for update", Logger.LogLevel.Warning);
                    return null;
                }

                // Update common BaseEntity properties
                existingEntity.LastUpdMethodName = "Update";
                existingEntity.ErrorMessage = updatedEntity.ErrorMessage;

                // Call virtual method to update specific properties
                UpdateEntityProperties(existingEntity, updatedEntity);

                await _context.SaveChangesAsync();
                Logger.LogWithContext($"Successfully updated entity with ID: {id}", Logger.LogLevel.Info);
                return existingEntity;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
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
            return await _dbSet.FindAsync(id) != null;
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
                .Where(e => !e.Archived && !e.Disabled)
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
                Logger.LogWithContext($"Adding multiple entities of type {typeof(T).Name}, count: {entities.Count}", Logger.LogLevel.Info);
                
                foreach (var entity in entities)
                {
                    entity.LastUpdMethodName = "AddMultiple";
                    _dbSet.Add(entity);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                Logger.LogWithContext($"Successfully added {entities.Count} entities", Logger.LogLevel.Info);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                await transaction.RollbackAsync();
                Logger.LogWithContext("Transaction rolled back due to error", Logger.LogLevel.Warning);
                return false;
            }
        }
    }
}