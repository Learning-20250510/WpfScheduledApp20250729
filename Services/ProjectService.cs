using Microsoft.EntityFrameworkCore;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.Services
{
    internal class ProjectService : BaseService<Project>
    {
        public ProjectService(BaseDbContext context) : base(context)
        {
        }

        // Override the UpdateEntityProperties method to handle Project-specific properties
        protected override void UpdateEntityProperties(Project existingProject, Project updatedProject)
        {
            existingProject.ProjectName = updatedProject.ProjectName;
        }

        // Project-specific methods
        public async Task<Project?> GetProjectByNameAsync(string projectName)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.ProjectName == projectName);
        }

        public async Task<List<Project>> GetProjectsByNameContainsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(p => p.ProjectName.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<Project> GetOrCreateDefaultProjectAsync()
        {
            var defaultProject = await GetProjectByNameAsync("Default Project");

            if (defaultProject == null)
            {
                defaultProject = new Project
                {
                    ProjectName = "Default Project",
                    TouchedAt = DateTime.UtcNow,
                    LastUpdMethodName = "GetOrCreateDefault"
                };
                return await AddAsync(defaultProject);
            }

            return defaultProject;
        }

        public async Task<bool> IsProjectNameUniqueAsync(string projectName, int? excludeId = null)
        {
            var query = _dbSet.Where(p => p.ProjectName == projectName);
            
            if (excludeId.HasValue)
            {
                query = query.Where(p => EF.Property<int>(p, "Id") != excludeId.Value);
            }

            return !await query.AnyAsync();
        }
    }
}