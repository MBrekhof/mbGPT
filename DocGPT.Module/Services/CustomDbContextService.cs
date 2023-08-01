using DocGPT.Module.BusinessObjects;
using Microsoft.EntityFrameworkCore;

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
                var optionsBuilder = new DbContextOptionsBuilder<CustomDbContext>();
                optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=1Zaqwsx2;");
                optionsBuilder.UseChangeTrackingProxies();
                optionsBuilder.UseObjectSpaceLinkProxies();
                return new CustomDbContext(optionsBuilder.Options);
            }
        }
    
}
