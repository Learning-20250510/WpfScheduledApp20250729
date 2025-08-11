using Microsoft.EntityFrameworkCore;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WpfScheduledApp20250729.Services
{
    public class MotivationService : BaseService<Motivation>
    {
        public MotivationService(BaseDbContext context) : base(context)
        {
        }

        // Override the UpdateEntityProperties method to handle Motivation-specific properties
        protected override void UpdateEntityProperties(Motivation existingMotivation, Motivation updatedMotivation)
        {
            existingMotivation.MotivationName = updatedMotivation.MotivationName;
            existingMotivation.Description = updatedMotivation.Description;
            existingMotivation.Message = updatedMotivation.Message;
            existingMotivation.Icon = updatedMotivation.Icon;
            existingMotivation.Color = updatedMotivation.Color;
            existingMotivation.DisplayOrder = updatedMotivation.DisplayOrder;
        }

        // Motivation-specific methods (keeping existing public interface for backward compatibility)
        public async Task<Motivation> AddMotivationAsync(Motivation motivation)
        {
            return await AddAsync(motivation);
        }

        public async Task<Motivation?> GetMotivationByIdAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<List<Motivation>> GetAllMotivationsAsync()
        {
            return await GetAllAsync();
        }

        public async Task<Motivation?> GetMotivationByNameAsync(string motivationName)
        {
            return await _dbSet
                .FirstOrDefaultAsync(m => m.MotivationName == motivationName);
        }

        public async Task<Motivation?> UpdateMotivationAsync(int id, Motivation updatedMotivation)
        {
            return await UpdateAsync(id, updatedMotivation);
        }

        public async Task<bool> DeleteMotivationAsync(int id)
        {
            return await DeleteAsync(id);
        }

        // Additional Motivation-specific methods

        public async Task<List<Motivation>> GetMotivationsByColorAsync(string color)
        {
            return await _dbSet
                .Where(m => m.Color == color)
                .OrderBy(m => m.DisplayOrder)
                .ToListAsync();
        }

        public async Task<bool> IsMotivationNameUniqueAsync(string motivationName, int? excludeId = null)
        {
            var query = _dbSet.Where(m => m.MotivationName == motivationName);
            
            if (excludeId.HasValue)
            {
                query = query.Where(m => EF.Property<int>(m, "Id") != excludeId.Value);
            }

            return !await query.AnyAsync();
        }

        public async Task<int> GetMaxDisplayOrderAsync()
        {
            if (!await _dbSet.AnyAsync())
            {
                return 0;
            }
            
            return await _dbSet.MaxAsync(m => m.DisplayOrder);
        }
    }
}