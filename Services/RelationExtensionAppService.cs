using Microsoft.EntityFrameworkCore;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.Services
{
    internal class RelationExtensionAppService : BaseService<RelationExtensionApp>
    {
        public RelationExtensionAppService(DevelopmentContext context) : base(context)
        {
        }

        // Override the UpdateEntityProperties method to handle RelationExtensionApp-specific properties
        protected override void UpdateEntityProperties(RelationExtensionApp existingRelation, RelationExtensionApp updatedRelation)
        {
            existingRelation.Extension = updatedRelation.Extension;
            existingRelation.Application = updatedRelation.Application;
        }

        // RelationExtensionApp-specific methods
        public async Task<RelationExtensionApp?> GetRelationByExtensionAsync(string extension)
        {
            return await _dbSet
                .FirstOrDefaultAsync(r => r.Extension == extension);
        }

        public async Task<List<RelationExtensionApp>> GetRelationsByApplicationAsync(string application)
        {
            return await _dbSet
                .Where(r => r.Application == application)
                .ToListAsync();
        }

        public async Task<List<RelationExtensionApp>> GetRelationsByExtensionContainsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(r => r.Extension.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<List<RelationExtensionApp>> GetRelationsByApplicationContainsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(r => r.Application.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<bool> IsExtensionUniqueAsync(string extension, int? excludeId = null)
        {
            var query = _dbSet.Where(r => r.Extension == extension);
            
            if (excludeId.HasValue)
            {
                query = query.Where(r => EF.Property<int>(r, "Id") != excludeId.Value);
            }

            return !await query.AnyAsync();
        }

        public async Task<RelationExtensionApp> GetOrCreateRelationAsync(string extension, string application)
        {
            var existingRelation = await GetRelationByExtensionAsync(extension);

            if (existingRelation == null)
            {
                existingRelation = new RelationExtensionApp
                {
                    Extension = extension,
                    Application = application,
                    TouchedAt = DateTime.UtcNow,
                    LastUpdMethodName = "GetOrCreate"
                };
                return await AddAsync(existingRelation);
            }

            // Update application if it's different
            if (existingRelation.Application != application)
            {
                existingRelation.Application = application;
                existingRelation.TouchedAt = DateTime.UtcNow;
                existingRelation.LastUpdMethodName = "GetOrCreate_Update";
                await _context.SaveChangesAsync();
            }

            return existingRelation;
        }

        public async Task<List<string>> GetAllExtensionsAsync()
        {
            return await _dbSet
                .Select(r => r.Extension)
                .Distinct()
                .OrderBy(e => e)
                .ToListAsync();
        }

        public async Task<List<string>> GetAllApplicationsAsync()
        {
            return await _dbSet
                .Select(r => r.Application)
                .Distinct()
                .OrderBy(a => a)
                .ToListAsync();
        }

        public async Task<Dictionary<string, List<string>>> GetExtensionApplicationMappingAsync()
        {
            var relations = await _dbSet.ToListAsync();
            
            return relations
                .GroupBy(r => r.Extension)
                .ToDictionary(
                    g => g.Key, 
                    g => g.Select(r => r.Application).Distinct().ToList()
                );
        }

        public async Task<Dictionary<string, int>> GetApplicationUsageCountAsync()
        {
            return await _dbSet
                .GroupBy(r => r.Application)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<List<RelationExtensionApp>> GetCommonFileExtensionsAsync()
        {
            var commonExtensions = new[] 
            { 
                ".txt", ".pdf", ".doc", ".docx", ".xls", ".xlsx", 
                ".ppt", ".pptx", ".jpg", ".png", ".gif", ".mp4", 
                ".mp3", ".zip", ".rar", ".exe", ".msi" 
            };
            
            return await _dbSet
                .Where(r => commonExtensions.Contains(r.Extension))
                .OrderBy(r => r.Extension)
                .ToListAsync();
        }

        public async Task<List<RelationExtensionApp>> GetRelationsForFileAsync(string fileName)
        {
            var extension = System.IO.Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
                return new List<RelationExtensionApp>();

            return await _dbSet
                .Where(r => r.Extension == extension)
                .ToListAsync();
        }

        public async Task<string?> GetRecommendedApplicationForFileAsync(string fileName)
        {
            var relations = await GetRelationsForFileAsync(fileName);
            
            // Return the first application found, or null if none
            return relations.FirstOrDefault()?.Application;
        }

        public async Task<bool> RegisterFileExtensionAsync(string fileName, string application)
        {
            var extension = System.IO.Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
                return false;

            await GetOrCreateRelationAsync(extension, application);
            return true;
        }

        public async Task<List<RelationExtensionApp>> GetOrphanedExtensionsAsync()
        {
            // Extensions that might have empty or invalid applications
            return await _dbSet
                .Where(r => string.IsNullOrEmpty(r.Application) || r.Application.Trim() == "")
                .ToListAsync();
        }

        public async Task<bool> UpdateApplicationForExtensionAsync(string extension, string newApplication)
        {
            var relation = await GetRelationByExtensionAsync(extension);
            if (relation == null) return false;

            relation.Application = newApplication;
            relation.TouchedAt = DateTime.UtcNow;
            relation.LastUpdMethodName = "UpdateApplication";

            await _context.SaveChangesAsync();
            return true;
        }

        // Bulk operations for file type management
        public async Task<bool> ImportFileAssociationsAsync(Dictionary<string, string> extensionAppPairs)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var pair in extensionAppPairs)
                {
                    await GetOrCreateRelationAsync(pair.Key, pair.Value);
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