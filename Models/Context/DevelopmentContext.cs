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
    internal class DevelopmentContext : DbContext
    {
        public DevelopmentContext(DbContextOptions<DevelopmentContext> options) : base(options) { }
        public DevelopmentContext() { }

        // 同じDbSetプロパティ...
        public DbSet<HighTask> HighTasks { get; set; }
        public DbSet<Architecture> Architectures { get; set; }
        public DbSet<HowToLearn> HowToLearns { get; set; }
        public DbSet<LastUpdPgm> LastUpdPgms { get; set; }
        public DbSet<PeriodicallyCycle> PeriodicallyCycles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskSpecifiedDate> TaskSpecifiedDates { get; set; }
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

                string connectionString = config.GetConnectionString("DevelopmentConnection");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}
