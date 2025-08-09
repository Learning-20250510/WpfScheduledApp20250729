using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WpfScheduledApp20250729.Models.Entities;


namespace WpfScheduledApp20250729.Models.Context
{
    public class DevelopmentContext : DbContext
    {
        public DevelopmentContext(DbContextOptions<DevelopmentContext> options) : base(options) { }
        public DevelopmentContext() { }

        // 同じDbSetプロパティ...
        public DbSet<HighTask> HighTasks { get; set; }
        public DbSet<Architecture> Architectures { get; set; }
        public DbSet<HowToLearn> HowToLearns { get; set; }
        public DbSet<PeriodicallyCycle> PeriodicallyCycles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<MiddleTask> MiddleTasks { get; set; }
        public DbSet<LowTask> LowTasks { get; set; }
        public DbSet<Webpage> Webpages { get; set; }
        public DbSet<RelationExtensionApp> RelationExtensionApps { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = config.GetConnectionString("DevelopmentConnection") 
                    ?? throw new InvalidOperationException("Connection string 'DevelopmentConnection' not found.");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.TouchedAt = now;
                        if (string.IsNullOrEmpty(entry.Entity.LastUpdMethodName))
                        {
                            entry.Entity.LastUpdMethodName = "Insert";
                        }
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.TouchedAt = now;
                        if (string.IsNullOrEmpty(entry.Entity.LastUpdMethodName))
                        {
                            entry.Entity.LastUpdMethodName = "Update";
                        }
                        break;
                }
            }
        }
    }
}
