using Microsoft.EntityFrameworkCore;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.Services
{
    internal class HighTaskService : BaseService<HighTask>
    {
        public HighTaskService(DevelopmentContext context) : base(context)
        {
        }

        // Override the UpdateEntityProperties method to handle HighTask-specific properties
        protected override void UpdateEntityProperties(HighTask existingTask, HighTask updatedTask)
        {
            existingTask.TaskName = updatedTask.TaskName;
            existingTask.Description = updatedTask.Description;
            existingTask.ArchitectureId = updatedTask.ArchitectureId;
            existingTask.ProjectId = updatedTask.ProjectId;
            existingTask.ClearTimesInTime = updatedTask.ClearTimesInTime;
            existingTask.ClearTimesOutofTime = updatedTask.ClearTimesOutofTime;
        }

        // HighTask-specific methods (keeping existing public interface for backward compatibility)
        public async Task<HighTask> AddHighTaskAsync(HighTask highTask)
        {
            return await AddAsync(highTask);
        }

        public async Task<HighTask?> GetHighTaskByIdAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<List<HighTask>> GetAllHighTasksAsync()
        {
            return await GetAllAsync();
        }

        public async Task<HighTask?> UpdateHighTaskAsync(int id, HighTask updatedTask)
        {
            return await UpdateAsync(id, updatedTask);
        }

        public async Task<bool> DeleteHighTaskAsync(int id)
        {
            return await DeleteAsync(id);
        }

        // Additional HighTask-specific methods
        public async Task<List<HighTask>> GetHighTasksByArchitectureIdAsync(int architectureId)
        {
            return await _dbSet
                .Where(ht => ht.ArchitectureId == architectureId)
                .ToListAsync();
        }

        public async Task<List<HighTask>> GetHighTasksByProjectIdAsync(int projectId)
        {
            return await _dbSet
                .Where(ht => ht.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<List<HighTask>> GetHighTasksByTaskNameContainsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(ht => ht.TaskName.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<List<HighTask>> GetHighTasksNeedingAttentionAsync()
        {
            return await _dbSet
                .Where(ht => ht.ClearTimesOutofTime > ht.ClearTimesInTime)
                .OrderByDescending(ht => ht.ClearTimesOutofTime)
                .ToListAsync();
        }

        public async Task<Dictionary<int, int>> GetTaskCountByArchitectureAsync()
        {
            return await _dbSet
                .GroupBy(ht => ht.ArchitectureId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<int, int>> GetTaskCountByProjectAsync()
        {
            return await _dbSet
                .GroupBy(ht => ht.ProjectId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<bool> MarkTaskCompletedAsync(int id, bool inTime)
        {
            var task = await _dbSet.FindAsync(id);
            if (task == null) return false;

            if (inTime)
            {
                task.ClearTimesInTime++;
            }
            else
            {
                task.ClearTimesOutofTime++;
            }
            
            task.TouchedAt = DateTime.UtcNow;
            task.LastUpdMethodName = "MarkCompleted";

            await _context.SaveChangesAsync();
            return true;
        }

        // Transaction methods using BaseService functionality
        public async Task<bool> AddMultipleHighTasksWithTransactionAsync(List<HighTask> tasks)
        {
            return await AddMultipleAsync(tasks);
        }

        // Complex transaction example for creating related tasks
        public async Task<bool> CreateHighTaskWithRelatedDataAsync(HighTask highTask, List<MiddleTask>? relatedMiddleTasks = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Add the main HighTask
                var addedHighTask = await AddAsync(highTask);

                // Add related MiddleTasks if provided
                if (relatedMiddleTasks != null && relatedMiddleTasks.Any())
                {
                    foreach (var middleTask in relatedMiddleTasks)
                    {
                        middleTask.HighTaskId = addedHighTask.Id;
                        _context.MiddleTasks.Add(middleTask);
                    }
                    await _context.SaveChangesAsync();
                }

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