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
                
                // エンティティが存在する場合、例外情報をErrorMessageに記録を試行
                try
                {
                    var entityToUpdate = await _dbSet.FindAsync(id);
                    if (entityToUpdate != null)
                    {
                        var errorMessage = FormatExceptionForErrorMessage(ex, "Update");
                        using var separateContext = (BaseDbContext)Activator.CreateInstance(_context.GetType())!; // 同じ型の別コンテキストインスタンス作成
                        var separateDbSet = separateContext.Set<T>();
                        var separateEntity = await separateDbSet.FindAsync(id);
                        
                        if (separateEntity != null)
                        {
                            separateEntity.ErrorMessage = errorMessage;
                            separateEntity.LastUpdMethodName = "UpdateError";
                            await separateContext.SaveChangesAsync();
                            Logger.LogWithContext($"Error message recorded for failed update on entity ID: {id}", Logger.LogLevel.Warning);
                        }
                    }
                }
                catch (Exception saveEx)
                {
                    Logger.LogError(saveEx, "RecordUpdateError", "BaseService");
                }
                
                throw;
            }
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                Logger.LogWithContext($"Deleting entity of type {typeof(T).Name} with ID: {id}", Logger.LogLevel.Info);
                var entity = await _dbSet.FindAsync(id);
                if (entity == null) 
                {
                    Logger.LogWithContext($"Entity with ID {id} not found for deletion", Logger.LogLevel.Warning);
                    return false;
                }

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                Logger.LogWithContext($"Successfully deleted entity with ID: {id}", Logger.LogLevel.Info);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                
                // 削除失敗時、エンティティにエラー情報を記録を試行
                try
                {
                    var errorMessage = FormatExceptionForErrorMessage(ex, "Delete");
                    using var separateContext = (BaseDbContext)Activator.CreateInstance(_context.GetType())!;
                    var separateDbSet = separateContext.Set<T>();
                    var separateEntity = await separateDbSet.FindAsync(id);
                    
                    if (separateEntity != null)
                    {
                        separateEntity.ErrorMessage = errorMessage;
                        separateEntity.LastUpdMethodName = "DeleteError";
                        await separateContext.SaveChangesAsync();
                        Logger.LogWithContext($"Error message recorded for failed delete on entity ID: {id}", Logger.LogLevel.Warning);
                    }
                }
                catch (Exception saveEx)
                {
                    Logger.LogError(saveEx, "RecordDeleteError", "BaseService");
                }
                
                throw;
            }
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.FindAsync(id) != null;
        }

        // Archive/Disable operations (soft delete alternatives)
        public virtual async Task<bool> ArchiveAsync(int id)
        {
            try
            {
                Logger.LogWithContext($"Archiving entity of type {typeof(T).Name} with ID: {id}", Logger.LogLevel.Info);
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    Logger.LogWithContext($"Entity with ID {id} not found for archive", Logger.LogLevel.Warning);
                    return false;
                }

                entity.Archived = true;
                entity.LastUpdMethodName = "Archive";

                await _context.SaveChangesAsync();
                Logger.LogWithContext($"Successfully archived entity with ID: {id}", Logger.LogLevel.Info);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                
                // アーカイブ失敗時、エンティティにエラー情報を記録を試行
                try
                {
                    var errorMessage = FormatExceptionForErrorMessage(ex, "Archive");
                    using var separateContext = (BaseDbContext)Activator.CreateInstance(_context.GetType())!;
                    var separateDbSet = separateContext.Set<T>();
                    var separateEntity = await separateDbSet.FindAsync(id);
                    
                    if (separateEntity != null)
                    {
                        separateEntity.ErrorMessage = errorMessage;
                        separateEntity.LastUpdMethodName = "ArchiveError";
                        await separateContext.SaveChangesAsync();
                        Logger.LogWithContext($"Error message recorded for failed archive on entity ID: {id}", Logger.LogLevel.Warning);
                    }
                }
                catch (Exception saveEx)
                {
                    Logger.LogError(saveEx, "RecordArchiveError", "BaseService");
                }
                
                throw;
            }
        }

        public virtual async Task<bool> DisableAsync(int id)
        {
            try
            {
                Logger.LogWithContext($"Disabling entity of type {typeof(T).Name} with ID: {id}", Logger.LogLevel.Info);
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    Logger.LogWithContext($"Entity with ID {id} not found for disable", Logger.LogLevel.Warning);
                    return false;
                }

                entity.Disabled = true;
                entity.LastUpdMethodName = "Disable";

                await _context.SaveChangesAsync();
                Logger.LogWithContext($"Successfully disabled entity with ID: {id}", Logger.LogLevel.Info);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                
                // 無効化失敗時、エンティティにエラー情報を記録を試行
                try
                {
                    var errorMessage = FormatExceptionForErrorMessage(ex, "Disable");
                    using var separateContext = (BaseDbContext)Activator.CreateInstance(_context.GetType())!;
                    var separateDbSet = separateContext.Set<T>();
                    var separateEntity = await separateDbSet.FindAsync(id);
                    
                    if (separateEntity != null)
                    {
                        separateEntity.ErrorMessage = errorMessage;
                        separateEntity.LastUpdMethodName = "DisableError";
                        await separateContext.SaveChangesAsync();
                        Logger.LogWithContext($"Error message recorded for failed disable on entity ID: {id}", Logger.LogLevel.Warning);
                    }
                }
                catch (Exception saveEx)
                {
                    Logger.LogError(saveEx, "RecordDisableError", "BaseService");
                }
                
                throw;
            }
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

        // Exception handling utility methods
        protected virtual string FormatExceptionForErrorMessage(Exception exception, string operation = "")
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var operationPrefix = string.IsNullOrEmpty(operation) ? "" : $"[{operation}] ";
            
            return $"{timestamp} {operationPrefix}{exception.GetType().Name}: {exception.Message}";
        }

        protected virtual async Task<bool> SaveEntityWithErrorMessage(T entity, string errorMessage)
        {
            try
            {
                entity.ErrorMessage = errorMessage;
                await _context.SaveChangesAsync();
                Logger.LogWithContext($"Error message saved for entity ID: {_context.Entry(entity).Property("Id").CurrentValue}", Logger.LogLevel.Warning);
                return true;
            }
            catch (Exception saveEx)
            {
                Logger.LogError(saveEx, "SaveEntityWithErrorMessage", "BaseService");
                return false;
            }
        }

        // Error message management methods
        public virtual async Task<bool> ClearErrorMessageAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null) return false;

                entity.ErrorMessage = null;
                entity.LastUpdMethodName = "ClearError";
                await _context.SaveChangesAsync();
                
                Logger.LogWithContext($"Error message cleared for entity ID: {id}", Logger.LogLevel.Info);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ClearErrorMessageAsync", "BaseService");
                return false;
            }
        }

        public virtual async Task<List<T>> GetEntitiesWithErrorsAsync()
        {
            try
            {
                return await _dbSet
                    .Where(e => e.ErrorMessage != null && e.ErrorMessage != "")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetEntitiesWithErrorsAsync", "BaseService");
                return new List<T>();
            }
        }
    }
}