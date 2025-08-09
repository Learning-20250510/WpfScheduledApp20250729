using Microsoft.EntityFrameworkCore;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.Services
{
    internal class ArchitectureService : BaseService<Architecture>
    {
        public ArchitectureService(BaseDbContext context) : base(context)
        {
        }

        // Override the UpdateEntityProperties method to handle Architecture-specific properties
        protected override void UpdateEntityProperties(Architecture existingArchitecture, Architecture updatedArchitecture)
        {
            existingArchitecture.ArchitectureName = updatedArchitecture.ArchitectureName;
        }

        // Architecture-specific methods (keeping existing public interface for backward compatibility)
        public async Task<Architecture> AddArchitectureAsync(Architecture architecture)
        {
            return await AddAsync(architecture);
        }

        public async Task<Architecture?> GetArchitectureByIdAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<List<Architecture>> GetAllArchitecturesAsync()
        {
            return await GetAllAsync();
        }

        public async Task<Architecture?> GetArchitectureByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.ArchitectureName == name);
        }

        public async Task<Architecture?> UpdateArchitectureAsync(int id, Architecture updatedArchitecture)
        {
            return await UpdateAsync(id, updatedArchitecture);
        }

        public async Task<bool> DeleteArchitectureAsync(int id)
        {
            return await DeleteAsync(id);
        }

        public new async Task<bool> ExistsAsync(int id)
        {
            return await base.ExistsAsync(id);
        }

        public async Task<Architecture> GetOrCreateDefaultArchitectureAsync()
        {
            var defaultArchitecture = await GetArchitectureByNameAsync("Action");
            
            if (defaultArchitecture == null)
            {
                defaultArchitecture = new Architecture
                {
                    ArchitectureName = "Action",
                    TouchedAt = DateTime.UtcNow,
                    LastUpdMethodName = "GetOrCreateDefault"
                };
                return await AddAsync(defaultArchitecture);
            }

            return defaultArchitecture;
        }

        // Additional Architecture-specific methods
        public async Task<List<Architecture>> GetArchitecturesByNameContainsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(a => a.ArchitectureName.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<bool> IsArchitectureNameUniqueAsync(string architectureName, int? excludeId = null)
        {
            var query = _dbSet.Where(a => a.ArchitectureName == architectureName);
            
            if (excludeId.HasValue)
            {
                query = query.Where(a => EF.Property<int>(a, "Id") != excludeId.Value);
            }

            return !await query.AnyAsync();
        }
    }
}