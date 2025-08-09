using Microsoft.EntityFrameworkCore;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.Services
{
    internal class HowToLearnService : BaseService<HowToLearn>
    {
        public HowToLearnService(BaseDbContext context) : base(context)
        {
        }

        // Override the UpdateEntityProperties method to handle HowToLearn-specific properties
        protected override void UpdateEntityProperties(HowToLearn existingHowToLearn, HowToLearn updatedHowToLearn)
        {
            existingHowToLearn.Htl = updatedHowToLearn.Htl;
        }

        // HowToLearn-specific methods
        public async Task<HowToLearn?> GetHowToLearnByNameAsync(string htl)
        {
            return await _dbSet
                .FirstOrDefaultAsync(h => h.Htl == htl);
        }

        public async Task<List<HowToLearn>> GetHowToLearnsByNameContainsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(h => h.Htl.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<HowToLearn> GetOrCreateDefaultHowToLearnAsync()
        {
            var defaultHowToLearn = await GetHowToLearnByNameAsync("Default How To Learn");

            if (defaultHowToLearn == null)
            {
                defaultHowToLearn = new HowToLearn
                {
                    Htl = "Default How To Learn",
                    TouchedAt = DateTime.UtcNow,
                    LastUpdMethodName = "GetOrCreateDefault"
                };
                return await AddAsync(defaultHowToLearn);
            }

            return defaultHowToLearn;
        }

        public async Task<bool> IsHtlUniqueAsync(string htl, int? excludeId = null)
        {
            var query = _dbSet.Where(h => h.Htl == htl);
            
            if (excludeId.HasValue)
            {
                query = query.Where(h => EF.Property<int>(h, "Id") != excludeId.Value);
            }

            return !await query.AnyAsync();
        }

        public async Task<List<HowToLearn>> GetHowToLearnsByIdsAsync(List<int> ids)
        {
            return await _dbSet
                .Where(h => ids.Contains(EF.Property<int>(h, "Id")))
                .ToListAsync();
        }
    }
}