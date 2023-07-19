using DevExpress.ExpressApp.EFCore.Updating;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.EFCore.DesignTime;

namespace DocGPT.Module.BusinessObjects;

// This code allows our Model Editor to get relevant EF Core metadata at design time.
// For details, please refer to https://supportcenter.devexpress.com/ticket/details/t933891.
public class DocGPTContextInitializer : DbContextTypesInfoInitializerBase {
	protected override DbContext CreateDbContext() {
		var optionsBuilder = new DbContextOptionsBuilder<DocGPTEFCoreDbContext>()
            .UseSqlServer(";")
            .UseChangeTrackingProxies()
            .UseObjectSpaceLinkProxies();
        return new DocGPTEFCoreDbContext(optionsBuilder.Options);
	}
}
//This factory creates DbContext for design-time services. For example, it is required for database migration.
public class DocGPTDesignTimeDbContextFactory : IDesignTimeDbContextFactory<DocGPTEFCoreDbContext> {
	public DocGPTEFCoreDbContext CreateDbContext(string[] args) {
		//throw new InvalidOperationException("Make sure that the database connection string and connection provider are correct. After that, uncomment the code below and remove this exception.");
		var optionsBuilder = new DbContextOptionsBuilder<DocGPTEFCoreDbContext>();
		optionsBuilder.UseSqlServer("Encrypt=false;Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=BCH-BTO;Initial Catalog=E965_EFCore");
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
    public DbSet<FileSystemStoreObjectDemo> FileSystemStoreObjectDemo { get; set; }
    //public DbSet<SplitAndEmbed> SplitAndEmbed { get; set; }
    public DbSet<Article> Article { get; set; }
    public DbSet<ArticleDetail> ArticleDetail { get; set; }
    public DbSet<ArticleVectorData> ArticleVectorData { get; set; }
    public DbSet<Chat> Chat { get; set; }
    public DbSet<Prompt> Prompt { get; set; }

    public  DbSet<CodeObject> CodeObject { get; set; }
    public DbSet<CodeObjectCategory> CodeObjectCategory { get; set; }

    [DbFunction("SimilarContentArticles", "dbo")]
    public IQueryable<SimilarContentArticlesResult> SimilarContentArticles(string vector)
    {
        return FromExpression(() => SimilarContentArticles(vector));
    }

    [DbFunction("SimilarContentCodeObject", "dbo")]
    public IQueryable<SimilarContentArticlesResult> SimilarContentCodeObject(string vector)
    {
        return FromExpression(() => SimilarContentCodeObject(vector));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        modelBuilder.UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);
    }
    protected void OnModelCreatingGeneratedFunctions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SimilarContentArticlesResult>().HasNoKey();
    }
}
