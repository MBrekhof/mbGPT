using DocGPT.Module.BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pgvector.EntityFrameworkCore;

namespace DocGPT.Module.Services
{

        public class CustomDbContext : DbContext
        {
            public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options)
            {
            }
            public DbSet<SimilarContentArticlesResult> SimilarContentArticlesResult { get; set; }
            public CustomDbContext CreateDbContext()//string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json") // Update the file name as needed
        .Build();

            string connectionString = configuration.GetConnectionString("ConnectionString");
            var optionsBuilder = new DbContextOptionsBuilder<CustomDbContext>();
                optionsBuilder.UseNpgsql(connectionString, o => o.UseVector()).UseLowerCaseNamingConvention();
                optionsBuilder.UseChangeTrackingProxies();
                optionsBuilder.UseObjectSpaceLinkProxies();
                return new CustomDbContext(optionsBuilder.Options);
            }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json") // Update the file name as needed
            .Build();

            string connectionString = configuration.GetConnectionString("ConnectionString");

            optionsBuilder.UseNpgsql(connectionString, o => o.UseVector()).UseLowerCaseNamingConvention();
        }
    }
    
}
