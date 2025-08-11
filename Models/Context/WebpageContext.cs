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

        public DbSet<Webpage> Webpages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = config.GetConnectionString("WebPageConnection") 
                    ?? throw new InvalidOperationException("Connection string 'WebPageConnection' not found.");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

    }
}
