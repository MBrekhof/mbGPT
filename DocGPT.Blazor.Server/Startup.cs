using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.Persistent.Base;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.EntityFrameworkCore;
using DocGPT.Blazor.Server.Services;
using DocGPT.Module.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.CodeAnalysis;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DocGPT.Module.BusinessObjects;

namespace DocGPT.Blazor.Server;

public class Startup {
    private string connectionString;

    public Startup(IConfiguration configuration) {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services) {
        services.AddSingleton(typeof(Microsoft.AspNetCore.SignalR.HubConnectionHandler<>), typeof(ProxyHubConnectionHandler<>));
        
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddHttpContextAccessor();
        services.AddScoped<CircuitHandler, CircuitHandlerProxy>();
        services.AddXaf(Configuration, builder => {
            builder.UseApplication<DocGPTBlazorApplication>();
            builder.Modules
                .AddConditionalAppearance()
                .AddFileAttachments()
                .AddOffice()
                .AddValidation(options => {
                    options.AllowValidationDetailsAccess = false;
                })
                .AddViewVariants()
                .Add<DocGPT.Module.DocGPTModule>()
            	.Add<DocGPTBlazorModule>();
            builder.ObjectSpaceProviders
               .AddSecuredEFCore(options => options.PreFetchReferenceProperties())
                   .WithDbContext<DocGPTEFCoreDbContext>((serviceProvider, options) => {
                        // Uncomment this code to use an in-memory database. This database is recreated each time the server starts. With the in-memory database, you don't need to make a migration when the data model is changed.
                        // Do not use this code in production environment to avoid data loss.
                        // We recommend that you refer to the following help topic before you use an in-memory database: https://docs.microsoft.com/en-us/ef/core/testing/in-memory
                        //options.UseInMemoryDatabase("InMemory");
                        //string connectionString = null;
                        if(Configuration.GetConnectionString("ConnectionString") != null) {
                            connectionString = Configuration.GetConnectionString("ConnectionString");
                        }
#if EASYTEST
                        if(Configuration.GetConnectionString("EasyTestConnectionString") != null) {
                            connectionString = Configuration.GetConnectionString("EasyTestConnectionString");
                        }
#endif
                        ArgumentNullException.ThrowIfNull(connectionString);
                       // options.UseSqlServer(connectionString);
                       options.UseNpgsql(connectionString).UseLowerCaseNamingConvention();
                       options.UseChangeTrackingProxies();
                        options.UseObjectSpaceLinkProxies();
                        options.UseLazyLoadingProxies();
                    })
                .AddNonPersistent();
                   builder.Security
                       .UseIntegratedMode(options => {
                           options.RoleType = typeof(PermissionPolicyRole);
                           options.UserType = typeof(ApplicationUser);
                           options.UserLoginInfoType = typeof(ApplicationUserLoginInfo);
                           options.SupportNavigationPermissionsForTypes = false;
                       })
                   .AddPasswordAuthentication(options => options.IsSupportChangePassword = true);
               });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.LoginPath = "/LoginPage";
                });
            // upload max to 50 Mb
            services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 50 * 1024 * 1024;
        });

        if (Configuration.GetConnectionString("ConnectionString") != null)
        {
            connectionString = Configuration.GetConnectionString("ConnectionString");
        }
        services.AddDbContext<CustomDbContext>(options =>options.UseNpgsql(connectionString).UseLowerCaseNamingConvention());
        services.AddScoped<SettingsService>();
        services.AddScoped<VectorService>();
        services.AddScoped<OpenAILLMService>();
        services.AddScoped<IMailService, MailService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if(env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }
        else {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. To change this for production scenarios, see: https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseRequestLocalization();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseXaf();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => {
            endpoints.MapXafEndpoints();
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
            endpoints.MapControllers();
        });

    }
}
