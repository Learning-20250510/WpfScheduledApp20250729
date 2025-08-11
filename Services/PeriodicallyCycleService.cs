using Microsoft.EntityFrameworkCore;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.Services
{
    internal class PeriodicallyCycleService : BaseService<PeriodicallyCycle>
    {
        public PeriodicallyCycleService(BaseDbContext context) : base(context)
        {
        }

        // Override the UpdateEntityProperties method to handle PeriodicallyCycle-specific properties
        protected override void UpdateEntityProperties(PeriodicallyCycle existingCycle, PeriodicallyCycle updatedCycle)
        {
            existingCycle.Cycle = updatedCycle.Cycle;
        }

        // PeriodicallyCycle-specific methods
        public async Task<PeriodicallyCycle?> GetPeriodicallyCycleByCycleAsync(string cycle)
        {
            return await _dbSet
                .FirstOrDefaultAsync(pc => pc.Cycle == cycle);
        }

        public async Task<List<PeriodicallyCycle>> GetPeriodicallyCyclesByCycleContainsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(pc => pc.Cycle.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<PeriodicallyCycle> GetOrCreateDefaultCycleAsync()
        {
            var defaultCycle = await GetPeriodicallyCycleByCycleAsync("Daily");

            if (defaultCycle == null)
            {
                defaultCycle = new PeriodicallyCycle
                {
                    Cycle = "Daily",
                    TouchedAt = DateTime.UtcNow,
                    LastUpdMethodName = "GetOrCreateDefault"
                };
                return await AddAsync(defaultCycle);
            }

            return defaultCycle;
        }

        public async Task<bool> IsCycleUniqueAsync(string cycle, int? excludeId = null)
        {
            var query = _dbSet.Where(pc => pc.Cycle == cycle);
            
            if (excludeId.HasValue)
            {
                query = query.Where(pc => EF.Property<int>(pc, "Id") != excludeId.Value);
            }

            return !await query.AnyAsync();
        }

        public async Task<List<PeriodicallyCycle>> GetCommonCyclesAsync()
        {
            var commonCycles = new[] { "Daily", "Weekly", "Monthly", "Yearly" };
            return await _dbSet
                .Where(pc => commonCycles.Contains(pc.Cycle))
                .OrderBy(pc => pc.Cycle)
                .ToListAsync();
        }

        public async Task<PeriodicallyCycle> GetOrCreateCycleAsync(string cycleName)
        {
            var existingCycle = await GetPeriodicallyCycleByCycleAsync(cycleName);

            if (existingCycle == null)
            {
                existingCycle = new PeriodicallyCycle
                {
                    Cycle = cycleName,
                    TouchedAt = DateTime.UtcNow,
                    LastUpdMethodName = "GetOrCreate"
                };
                return await AddAsync(existingCycle);
            }

            return existingCycle;
        }
    }
}