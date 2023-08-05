﻿using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using Microsoft.EntityFrameworkCore;
using DevExpress.ExpressApp.EFCore;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Design;
using DocGPT.Module.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.CodeAnalysis;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DocGPT.Module.BusinessObjects;

namespace DocGPT.Win;

public class ApplicationBuilder : IDesignTimeApplicationFactory {
    public static WinApplication BuildApplication(string connectionString) {
        var builder = WinApplication.CreateBuilder();
        // Register custom services for Dependency Injection. For more information, refer to the following topic: https://docs.devexpress.com/eXpressAppFramework/404430/
       
        // Register 3rd-party IoC containers (like Autofac, Dryloc, etc.)
        // builder.UseServiceProviderFactory(new DryIocServiceProviderFactory());
        // builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        builder.UseApplication<DocGPTWindowsFormsApplication>();
        builder.Modules
            .AddConditionalAppearance()
            .AddFileAttachments()
            .AddOffice()
            .AddTreeListEditors()
            .AddValidation(options => {
                options.AllowValidationDetailsAccess = false;
            })
            .AddViewVariants()
            .Add<DocGPT.Module.DocGPTModule>()
        	.Add<DocGPTWinModule>();
        builder.ObjectSpaceProviders
             .AddSecuredEFCore(options => options.PreFetchReferenceProperties())
                .WithDbContext<DocGPTEFCoreDbContext>((application, options) => {
                    // Uncomment this code to use an in-memory database. This database is recreated each time the server starts. With the in-memory database, you don't need to make a migration when the data model is changed.
                    // Do not use this code in production environment to avoid data loss.
                    // We recommend that you refer to the following help topic before you use an in-memory database: https://docs.microsoft.com/en-us/ef/core/testing/in-memory
                    //options.UseInMemoryDatabase("InMemory");
                    //options.UseSqlServer(connectionString);
                    options.UseNpgsql(connectionString).UseLowerCaseNamingConvention();
                    options.UseChangeTrackingProxies();
                    options.UseObjectSpaceLinkProxies();
                })
            .AddNonPersistent();
        builder.Security
    .UseIntegratedMode(options => {
        options.RoleType = typeof(PermissionPolicyRole);
        options.UserType = typeof(ApplicationUser);
        options.UserLoginInfoType = typeof(ApplicationUserLoginInfo);
    })
    .UsePasswordAuthentication();
        builder.AddBuildStep(application =>
        {
            application.ConnectionString = connectionString;
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached && application.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)
            {
                application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
        });
        builder.Services.AddDbContext<CustomDbContext>(options =>
        options.UseNpgsql("Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=1Zaqwsx2;").UseLowerCaseNamingConvention());
        builder.Services.AddScoped<SettingsService>();
        builder.Services.AddScoped<VectorService>();
        builder.Services.AddScoped<OpenAILLMService>();
        builder.Services.AddScoped<IMailService, MailService>();
        var winApplication = builder.Build();
        return winApplication;
    }

    XafApplication IDesignTimeApplicationFactory.Create()
        => BuildApplication(XafApplication.DesignTimeConnectionString);
}
