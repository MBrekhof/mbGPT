﻿using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.EFCore.DesignTime;
using DevExpress.Persistent.BaseImpl.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Logging;
using Pgvector.EntityFrameworkCore;

namespace DocGPT.Module.BusinessObjects;

// This code allows our Model Editor to get relevant EF Core metadata at design time.
// For details, please refer to https://supportcenter.devexpress.com/ticket/details/t933891.
public class DocGPTContextInitializer : DbContextTypesInfoInitializerBase {
	protected override DbContext CreateDbContext() {
		var optionsBuilder = new DbContextOptionsBuilder<DocGPTEFCoreDbContext>()
            //.UseSqlServer(";")
            .UseNpgsql(";")
            .UseChangeTrackingProxies()
            .UseObjectSpaceLinkProxies();
        return new DocGPTEFCoreDbContext(optionsBuilder.Options);
	}
}
//This factory creates DbContext for design-time services. For example, it is required for database migration.
public class DocGPTDesignTimeDbContextFactory : IDesignTimeDbContextFactory<DocGPTEFCoreDbContext> {
	public DocGPTEFCoreDbContext CreateDbContext(string[] args) {
        //throw new InvalidOperationException("Make sure that the database connection string and connection provider are correct. After that, uncomment the code below and remove this exception.");
        var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        // Get connection string from configuration
        var connectionString = configuration.GetConnectionString("ConnectionString");

        var optionsBuilder = new DbContextOptionsBuilder<DocGPTEFCoreDbContext>();
        //optionsBuilder.UseSqlServer("Encrypt=false;Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=BCH-BTO;Initial Catalog=E965_EFCore");
        //TODO: get this from a config file?
        optionsBuilder.UseNpgsql(connectionString,o => o.UseVector()).UseLowerCaseNamingConvention();
        optionsBuilder.UseChangeTrackingProxies();
		optionsBuilder.UseObjectSpaceLinkProxies();
		return new DocGPTEFCoreDbContext(optionsBuilder.Options);
	}
}
[TypesInfoInitializer(typeof(DocGPTContextInitializer))]
public class DocGPTEFCoreDbContext : DbContext {
	public DocGPTEFCoreDbContext(DbContextOptions<DocGPTEFCoreDbContext> options) : base(options) {
	}
	//public DbSet<ModuleInfo> ModulesInfo { get; set; }
	public DbSet<FileData> FileData { get; set; }
    public DbSet<FileSystemStoreObject> FileSystemStoreObject { get; set; }

    public DbSet<Article> Article { get; set; }
    public DbSet<ArticleDetail> ArticleDetail { get; set; }

    public DbSet<Chat> Chat { get; set; }
    public DbSet<Prompt> Prompt { get; set; }

    public DbSet<CodeObject> CodeObject { get; set; }
    public DbSet<CodeObjectCategory> CodeObjectCategory { get; set; }

    public DbSet<WebSiteData> WebSiteData { get; set; }

    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ApplicationUserLoginInfo> UserLoginInfos { get; set; }

    public DbSet<Settings> Settings { get; set; }
    public DbSet<ChatModel> ChatModel { get; set; }

    public DbSet<EmbeddingModel> EmbeddingModel { get; set; }
    public DbSet<MailData> MailData { get; set; }
    public DbSet<SimilarContentArticlesResult> SimilarContentArticlesResult { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        modelBuilder.UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);
        modelBuilder.Entity<Article>()
            .HasMany(e => e.ArticleDetail)
            .WithOne(e => e.Article)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
    //protected void OnModelCreatingGeneratedFunctions(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<SimilarContentArticlesResult>().HasNoKey();
    //}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        // Get connection string from configuration
        var connectionString = configuration.GetConnectionString("ConnectionString");

        //optionsBuilder.UseSqlServer("Encrypt=false;Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=BCH-BTO;Initial Catalog=E965_EFCore");
        //TODO: get this from a config file?
        optionsBuilder.UseNpgsql(connectionString, o => o.UseVector()).UseLowerCaseNamingConvention();
    }
    public static readonly ILoggerFactory MyLoggerFactory
    = LoggerFactory.Create(builder => { builder.AddDebug(); });
}
