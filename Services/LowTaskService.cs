using Microsoft.EntityFrameworkCore;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.Services
{
    public class LowTaskService : BaseService<LowTask>
    {
        public LowTaskService(BaseDbContext context) : base(context)
        {
        }

        // Override the UpdateEntityProperties method to handle LowTask-specific properties
        protected override void UpdateEntityProperties(LowTask existingTask, LowTask updatedTask)
        {
            existingTask.MiddleTaskId = updatedTask.MiddleTaskId;
            existingTask.ProjectId = updatedTask.ProjectId;
            existingTask.EstimatedTime = updatedTask.EstimatedTime;
            existingTask.Description = updatedTask.Description;
            existingTask.ExecutionDate = updatedTask.ExecutionDate;
            existingTask.ExecutionTime = updatedTask.ExecutionTime;
            existingTask.CanAutoReschedule = updatedTask.CanAutoReschedule;
            existingTask.LastClearedAt = updatedTask.LastClearedAt;
            existingTask.ClearTimesInTime = updatedTask.ClearTimesInTime;
            existingTask.ClearTimesOutofTime = updatedTask.ClearTimesOutofTime;
        }

        // LowTask-specific methods
        public async Task<List<LowTask>> GetLowTasksByMiddleTaskIdAsync(int middleTaskId)
        {
            return await _dbSet
                .Where(lt => lt.MiddleTaskId == middleTaskId)
                .ToListAsync();
        }

        public async Task<List<LowTask>> GetLowTasksByProjectIdAsync(int projectId)
        {
            return await _dbSet
                .Where(lt => lt.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<List<LowTask>> GetLowTasksByDateAsync(DateOnly date)
        {
            return await _dbSet
                .Where(lt => lt.ExecutionDate == date)
                .OrderBy(lt => lt.ExecutionTime)
                .ToListAsync();
        }

        public async Task<List<LowTask>> GetLowTasksByDateRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            return await _dbSet
                .Where(lt => lt.ExecutionDate >= startDate && lt.ExecutionDate <= endDate)
                .OrderBy(lt => lt.ExecutionDate)
                .ThenBy(lt => lt.ExecutionTime)
                .ToListAsync();
        }

        public async Task<List<LowTask>> GetAutoReschedulableTasksAsync()
        {
            return await _dbSet
                .Where(lt => lt.CanAutoReschedule)
                .ToListAsync();
        }

        public async Task<List<LowTask>> GetTasksByEstimatedTimeRangeAsync(int minTime, int maxTime)
        {
            return await _dbSet
                .Where(lt => lt.EstimatedTime >= minTime && lt.EstimatedTime <= maxTime)
                .ToListAsync();
        }

        public async Task<List<LowTask>> GetOverdueTasksAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);

            return await _dbSet
                .Where(lt => lt.ExecutionDate < today || 
                           (lt.ExecutionDate == today && lt.ExecutionTime < currentTime))
                .OrderBy(lt => lt.ExecutionDate)
                .ThenBy(lt => lt.ExecutionTime)
                .ToListAsync();
        }

        public async Task<List<LowTask>> GetUpcomingTasksAsync(int days = 7)
        {
            var endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(days));
            var today = DateOnly.FromDateTime(DateTime.Today);

            return await _dbSet
                .Where(lt => lt.ExecutionDate >= today && lt.ExecutionDate <= endDate)
                .OrderBy(lt => lt.ExecutionDate)
                .ThenBy(lt => lt.ExecutionTime)
                .ToListAsync();
        }

        public async Task<bool> MarkTaskCompletedAsync(int id, bool inTime)
        {
            var task = await _dbSet.FindAsync(id);
            if (task == null) return false;

            task.LastClearedAt = DateTime.UtcNow;
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

        public async Task<bool> RescheduleTaskAsync(int id, DateOnly newDate, TimeOnly newTime)
        {
            var task = await _dbSet.FindAsync(id);
            if (task == null) return false;

            task.ExecutionDate = newDate;
            task.ExecutionTime = newTime;
            task.TouchedAt = DateTime.UtcNow;
            task.LastUpdMethodName = "Reschedule";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Dictionary<int, int>> GetTaskCountByProjectAsync()
        {
            return await _dbSet
                .GroupBy(lt => lt.ProjectId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<int, int>> GetTaskCountByMiddleTaskAsync()
        {
            return await _dbSet
                .GroupBy(lt => lt.MiddleTaskId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<List<LowTask>> GetTasksNeedingAttentionAsync()
        {
            return await _dbSet
                .Where(lt => lt.ClearTimesOutofTime > lt.ClearTimesInTime)
                .OrderByDescending(lt => lt.ClearTimesOutofTime)
                .ToListAsync();
        }

        // Transaction operations
        public async Task<bool> AddMultipleLowTasksAsync(List<LowTask> tasks)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var task in tasks)
                {
                    _dbSet.Add(task);
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