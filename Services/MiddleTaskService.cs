using Microsoft.EntityFrameworkCore;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.Services
{
    public class MiddleTaskService : BaseService<MiddleTask>
    {
        public MiddleTaskService(BaseDbContext context) : base(context)
        {
        }

        // Override the UpdateEntityProperties method to handle MiddleTask-specific properties
        protected override void UpdateEntityProperties(MiddleTask existingTask, MiddleTask updatedTask)
        {
            existingTask.HighTaskId = updatedTask.HighTaskId;
            existingTask.HtlId = updatedTask.HtlId;
            existingTask.ProjectId = updatedTask.ProjectId;
            existingTask.Description = updatedTask.Description;
            existingTask.CanAutoReschedule = updatedTask.CanAutoReschedule;
            existingTask.EstimatedTime = updatedTask.EstimatedTime;
            existingTask.LastClearedAt = updatedTask.LastClearedAt;
            existingTask.ClearTimesInTime = updatedTask.ClearTimesInTime;
            existingTask.ClearTimesOutofTime = updatedTask.ClearTimesOutofTime;
            existingTask.PeriodicallyCyclesId = updatedTask.PeriodicallyCyclesId;
            existingTask.FileName = updatedTask.FileName;
            existingTask.Url = updatedTask.Url;
            existingTask.SpecifiedPageAsPdf = updatedTask.SpecifiedPageAsPdf;
            existingTask.SpecifiedScrollAmountAsUrl = updatedTask.SpecifiedScrollAmountAsUrl;
        }

        // MiddleTask-specific methods
        public async Task<List<MiddleTask>> GetMiddleTasksByHighTaskIdAsync(int highTaskId)
        {
            return await _dbSet
                .Where(mt => mt.HighTaskId == highTaskId)
                .ToListAsync();
        }

        public async Task<List<MiddleTask>> GetMiddleTasksByProjectIdAsync(int projectId)
        {
            return await _dbSet
                .Where(mt => mt.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<List<MiddleTask>> GetMiddleTasksByHtlIdAsync(int htlId)
        {
            return await _dbSet
                .Where(mt => mt.HtlId == htlId)
                .ToListAsync();
        }

        public async Task<List<MiddleTask>> GetMiddleTasksByPeriodicallyCycleIdAsync(int periodicallyCycleId)
        {
            return await _dbSet
                .Where(mt => mt.PeriodicallyCyclesId == periodicallyCycleId)
                .ToListAsync();
        }

        public async Task<List<MiddleTask>> GetAutoReschedulableTasksAsync()
        {
            return await _dbSet
                .Where(mt => mt.CanAutoReschedule)
                .ToListAsync();
        }

        public async Task<List<MiddleTask>> GetTasksByEstimatedTimeRangeAsync(int minTime, int maxTime)
        {
            return await _dbSet
                .Where(mt => mt.EstimatedTime >= minTime && mt.EstimatedTime <= maxTime)
                .ToListAsync();
        }

        public async Task<List<MiddleTask>> GetTasksWithUrlAsync()
        {
            return await _dbSet
                .Where(mt => !string.IsNullOrEmpty(mt.Url))
                .ToListAsync();
        }

        public async Task<List<MiddleTask>> GetTasksWithFileAsync()
        {
            return await _dbSet
                .Where(mt => !string.IsNullOrEmpty(mt.FileName))
                .ToListAsync();
        }

        public async Task<List<MiddleTask>> GetRecentlyCompletedTasksAsync(int days = 30)
        {
            var cutoffDate = DateTimeOffset.UtcNow.AddDays(-days);
            return await _dbSet
                .Where(mt => mt.LastClearedAt.HasValue && mt.LastClearedAt >= cutoffDate)
                .OrderByDescending(mt => mt.LastClearedAt)
                .ToListAsync();
        }

        public async Task<bool> MarkTaskCompletedAsync(int id, bool inTime)
        {
            var task = await _dbSet.FindAsync(id);
            if (task == null) return false;

            task.LastClearedAt = DateTimeOffset.UtcNow;
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

        public async Task<List<MiddleTask>> GetTasksNeedingAttentionAsync()
        {
            return await _dbSet
                .Where(mt => mt.ClearTimesOutofTime > mt.ClearTimesInTime)
                .OrderByDescending(mt => mt.ClearTimesOutofTime)
                .ToListAsync();
        }

        public async Task<Dictionary<int, int>> GetTaskCountByProjectAsync()
        {
            return await _dbSet
                .GroupBy(mt => mt.ProjectId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<int, int>> GetTaskCountByHtlAsync()
        {
            return await _dbSet
                .GroupBy(mt => mt.HtlId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        // Complex queries with joins (these would require related services or direct context queries)
        public async Task<List<MiddleTask>> GetTasksWithDetailsAsync()
        {
            // This method would typically include navigation properties or use related services
            // For now, returning basic tasks - in real implementation you might want to use Include
            return await _dbSet.ToListAsync();
        }
    }
}