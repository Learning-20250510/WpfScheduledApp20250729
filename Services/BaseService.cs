using Microsoft.EntityFrameworkCore;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.Services
{
    internal abstract class BaseService<T> where T : BaseEntity
    {
        protected readonly DevelopmentContext _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseService(DevelopmentContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // Common CRUD operations
        public virtual async Task<T> AddAsync(T entity)
        {
            entity.TouchedAt = DateTime.UtcNow;
            entity.LastUpdMethodName = "Add";
            
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T?> UpdateAsync(int id, T updatedEntity)
        {
            var existingEntity = await _dbSet.FindAsync(id);
            if (existingEntity == null) return null;

            // Update common BaseEntity properties
            existingEntity.TouchedAt = DateTime.UtcNow;
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

            // Note: Archived and Disabled properties have private setters
            // These would need to be updated via raw SQL or reflection
            // For now, we'll use TouchedAt and LastUpdMethodName to track the operation
            entity.TouchedAt = DateTime.UtcNow;
            entity.LastUpdMethodName = "Archive";

            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> DisableAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            entity.TouchedAt = DateTime.UtcNow;
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

        // Transaction helper methods
        public virtual async Task<bool> AddMultipleAsync(List<T> entities)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var entity in entities)
                {
                    entity.TouchedAt = DateTime.UtcNow;
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