using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.Models.Context
{
    internal class ProductionContext : BaseDbContext
    {
        public ProductionContext(DbContextOptions<ProductionContext> options) : base(options) { }
        public ProductionContext() : base() { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = config.GetConnectionString("ProductionConnection") 
                    ?? throw new InvalidOperationException("Connection string 'ProductionConnection' not found.");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}
