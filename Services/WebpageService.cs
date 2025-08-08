using Microsoft.EntityFrameworkCore;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.Services
{
    internal class WebpageService : BaseService<Webpage>
    {
        public WebpageService(DevelopmentContext context) : base(context)
        {
        }

        // Override the UpdateEntityProperties method to handle Webpage-specific properties
        protected override void UpdateEntityProperties(Webpage existingWebpage, Webpage updatedWebpage)
        {
            existingWebpage.Url = updatedWebpage.Url;
        }

        // Webpage-specific methods
        public async Task<Webpage?> GetWebpageByUrlAsync(string url)
        {
            return await _dbSet
                .FirstOrDefaultAsync(w => w.Url == url);
        }

        public async Task<List<Webpage>> GetWebpagesByUrlContainsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(w => w.Url.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<List<Webpage>> GetWebpagesByDomainAsync(string domain)
        {
            return await _dbSet
                .Where(w => w.Url.Contains(domain))
                .ToListAsync();
        }

        public async Task<bool> IsUrlUniqueAsync(string url, int? excludeId = null)
        {
            var query = _dbSet.Where(w => w.Url == url);
            
            if (excludeId.HasValue)
            {
                query = query.Where(w => EF.Property<int>(w, "Id") != excludeId.Value);
            }

            return !await query.AnyAsync();
        }

        public async Task<List<Webpage>> GetWebpagesByProtocolAsync(string protocol)
        {
            return await _dbSet
                .Where(w => w.Url.StartsWith(protocol))
                .ToListAsync();
        }

        public async Task<List<Webpage>> GetHttpsWebpagesAsync()
        {
            return await GetWebpagesByProtocolAsync("https://");
        }

        public async Task<List<Webpage>> GetHttpWebpagesAsync()
        {
            return await GetWebpagesByProtocolAsync("http://");
        }

        public async Task<Webpage> GetOrCreateWebpageAsync(string url)
        {
            var existingWebpage = await GetWebpageByUrlAsync(url);

            if (existingWebpage == null)
            {
                existingWebpage = new Webpage
                {
                    Url = url,
                    TouchedAt = DateTime.UtcNow,
                    LastUpdMethodName = "GetOrCreate"
                };
                return await AddAsync(existingWebpage);
            }

            return existingWebpage;
        }

        public async Task<List<Webpage>> GetWebpagesByUrlPatternAsync(string pattern)
        {
            // Simple pattern matching using LIKE-style patterns
            // Note: For more complex regex patterns, you might want to load data and filter in memory
            var likePattern = pattern.Replace("*", "%").Replace("?", "_");
            
            return await _dbSet
                .Where(w => EF.Functions.Like(w.Url, likePattern))
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetWebpageCountByDomainAsync()
        {
            var webpages = await _dbSet.ToListAsync();
            
            return webpages
                .GroupBy(w => ExtractDomainFromUrl(w.Url))
                .Where(g => !string.IsNullOrEmpty(g.Key))
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<List<Webpage>> GetRecentlyAddedWebpagesAsync(int days = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _dbSet
                .Where(w => w.TouchedAt >= cutoffDate)
                .OrderByDescending(w => w.TouchedAt)
                .ToListAsync();
        }

        // Helper method to extract domain from URL
        private static string ExtractDomainFromUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                return uri.Host;
            }
            catch
            {
                return string.Empty;
            }
        }

        public Task<bool> IsValidUrlAsync(string url)
        {
            var isValid = Uri.TryCreate(url, UriKind.Absolute, out var uri) && 
                   (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
            return Task.FromResult(isValid);
        }

        public async Task<List<Webpage>> GetWebpagesRequiringValidationAsync()
        {
            var webpages = await _dbSet.ToListAsync();
            
            return webpages
                .Where(w => !string.IsNullOrEmpty(w.ErrorMessage))
                .ToList();
        }

        public async Task<bool> ValidateAndUpdateWebpageAsync(int id, string? errorMessage = null)
        {
            var webpage = await _dbSet.FindAsync(id);
            if (webpage == null) return false;

            webpage.ErrorMessage = errorMessage;
            webpage.TouchedAt = DateTime.UtcNow;
            webpage.LastUpdMethodName = "ValidateAndUpdate";

            await _context.SaveChangesAsync();
            return true;
        }
    }
}