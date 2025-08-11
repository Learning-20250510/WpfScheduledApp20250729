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
    public class DevelopmentContext : BaseDbContext
    {
        public DevelopmentContext(DbContextOptions<DevelopmentContext> options) : base(options) { }
        public DevelopmentContext() : base() { }


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

    }
}
