using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfScheduledApp20250729.Models.Entities;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;

namespace WpfScheduledApp20250729.Models.Context
{
    internal class WebPageContext : DbContext
    {
        public WebPageContext(DbContextOptions<WebPageContext> options) : base(options) { }
        public WebPageContext() { }

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = config.GetConnectionString("WebPageConnection");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

    }
}
