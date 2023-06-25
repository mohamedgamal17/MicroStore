using Duende.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application;
using MicroStore.IdentityProvider.Identity.Infrastructure;
using MicroStore.IdentityProvider.IdentityServer.Infrastructure;
using MicroStore.IdentityProvider.IdentityServer.Infrastructure.EntityFramework;
using Serilog;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Microsoft.OpenApi.Models;
using MicroStore.BuildingBlocks.AspNetCore.Infrastructure;
using Volo.Abp.AutoMapper;
using Duende.IdentityServer.EntityFramework.Mappers;
using MicroStore.IdentityProvider.Host.Services;
using MicroStore.IdentityProvider.Identity.Infrastructure.EntityFramework;
using MicroStore.AspNetCore.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using MicroStore.IdentityProvider.Host.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using MicroStore.IdentityProvider.Host.Theming;
using Volo.Abp.UI.Navigation;
using MicroStore.IdentityProvider.Host.Menus;

namespace MicroStore.IdentityProvider.Host
{

    [DependsOn(typeof(IdentityServerInfrastrcutreModule),
        typeof(IdentityInfrastructureModule),
        typeof(MicroStoreAspNetCoreModule),
        typeof(AbpAutofacModule),
        typeof(MicroStoreAspNetCoreUIModule),
        typeof(AbpAspNetCoreMvcUiThemeSharedModule))]
    public class IdentityProviderHostModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            var config = context.Services.GetConfiguration();


            ConfigureAspNetIdentity(context.Services);

            ConfigureIdentityServer(config,context.Services);

            ConfigureExternalAuthentication(context.Services);

            ConfigureSwagger(context.Services);

            Configure<AbpAntiForgeryOptions>(opt =>
            {
                opt.AutoValidate = false;
            });

            Configure<AbpAutoMapperOptions>(opt => opt.AddMaps<IdentityProviderHostModule>());


            Configure<AbpNavigationOptions>(opt =>
            {
                opt.MainMenuNames.Add(ApplicationMenusDefaults.BackEnd);

                opt.MenuContributors.Add(new BackEndMenusContributor());
            });

            Configure<AbpBundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Configure(StandardBundles.Styles.Global, bundle => { bundle.AddContributors(typeof(ApplicationThemeGlobalStyleContributor)); });

                options
                    .ScriptBundles
                    .Configure(StandardBundles.Scripts.Global, bundle => bundle.AddContributors(typeof(ApplicationThemeGlobalScriptContributor)));
            });

            Configure<AbpThemingOptions>(opt =>
            {
                opt.DefaultThemeName = StandardApplicationTheme.Name;

                opt.Themes.Add<StandardApplicationTheme>();

            });
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            // Remove Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared Application Part

            Configure<IMvcBuilder>(opt =>
            {
                var appPart = opt.PartManager.ApplicationParts
                    .SingleOrDefault(x => ((AssemblyPart)x).Assembly == typeof(AbpAspNetCoreMvcUiThemeSharedModule).Assembly);

                if (appPart != null)
                {
                    opt.PartManager.ApplicationParts.Remove(appPart);
                }
            });
        }
        public override async Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                await PrepareDataBaseMigration(scope.ServiceProvider);


                var env = context.GetEnvironment();

                if (env.IsDevelopment())
                {
                     await SeedIdentityServerConfigurationData(context.ServiceProvider);

                    await SeedIdentityData(context.ServiceProvider);
                }

            }
       

       }

       
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {

            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
                
            app.UseSerilogRequestLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Provider API");
                });

            }


         //   app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();   
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseConfiguredEndpoints();
        }

       
        private async Task PrepareDataBaseMigration(IServiceProvider service)
        {
            using(var scope = service.CreateScope())
            {
                var identityDbContext = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
                var identityServerConfiguraytionDbContext = scope.ServiceProvider.GetRequiredService<ApplicationConfigurationDbContext>();
                var operationalDbContext = scope.ServiceProvider.GetRequiredService<ApplicationPersistedGrantDbContext>();
                await identityDbContext.Database.MigrateAsync();
                await identityServerConfiguraytionDbContext.Database.MigrateAsync();
                await operationalDbContext.Database.MigrateAsync();
            }

        }


        private async Task SeedIdentityServerConfigurationData(IServiceProvider service)
        {
            using (var scope = service.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationConfigurationDbContext>();

                if (! await context.Clients.AnyAsync())
                {
                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }
                if (! await context.IdentityResources.AnyAsync())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (! await context.ApiScopes.AnyAsync())
                {
                    foreach (var resource in Config.ApiScopes)
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if(!await context.ApiResources.AnyAsync())
                {
                    foreach (var resource in Config.ApiResources)
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

            }
        }


        private async Task SeedIdentityData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
                var usermanager = scope.ServiceProvider.GetRequiredService<ApplicationUserManager>();

                if (!await context.Users.AnyAsync())
                {
                    await Config.SeedIdentityUsers(scope.ServiceProvider);
                }
            }
        }

        private void ConfigureAspNetIdentity(IServiceCollection services)
        {
            services.AddIdentity<ApplicationIdentityUser, ApplicationIdentityRole>(opt =>
            {
                opt.Stores.ProtectPersonalData = false;
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
               // opt.ClaimsIdentity.MapClaimIdentity();
            }).AddRoleManager<ApplicationRoleManager>()
            .AddUserManager<ApplicationUserManager>()
            .AddDefaultTokenProviders();

            services.AddDataProtection();



        }

        private void ConfigureIdentityServer(IConfiguration configuration ,IServiceCollection services)
        {
            services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.EmitStaticAudienceClaim = true;
                })
                .AddServerSideSessions()
                .AddConfigurationStore<ApplicationConfigurationDbContext>(cfg =>
                {
                    cfg.DefaultSchema = IdentityServerDbConsts.ConfigurationSchema;

                    cfg.ConfigureDbContext = (builder) =>
                    {
                        builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlServerOpt =>
                        {
                            sqlServerOpt.MigrationsAssembly(typeof(IdentityServerInfrastrcutreModule).Assembly.FullName)
                                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                        });


                    };
                })
                .AddOperationalStore<ApplicationPersistedGrantDbContext>(cfg =>
                {

                    cfg.DefaultSchema = IdentityServerDbConsts.OperationalSchema;

                    cfg.ConfigureDbContext = (builder) =>
                    {
                        builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlServerOpt =>
                        {
                            sqlServerOpt.MigrationsAssembly(typeof(IdentityServerInfrastrcutreModule).Assembly.FullName)
                                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                        });
                    };
                }).AddAspNetIdentity<ApplicationIdentityUser>()
                .AddExtensionGrantValidator<TokenExchangeExtensionGrantValidator>()
                .AddProfileService<ApplicationProfileService>();
        }


        private void ConfigureExternalAuthentication(IServiceCollection services)
        {
            services.AddAuthentication()
              .AddGoogle(options =>
              {
                  options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                  // register your IdentityServer with Google at https://console.developers.google.com
                  // enable the Google+ API
                  // set the redirect URI to https://localhost:5001/signin-google
                  options.ClientId = "copy client ID from Google here";
                  options.ClientSecret = "copy client secret from Google here";
              });
        }


        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen((options) =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity provider Api", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                options.OperationFilter<AuthorizeCheckOperationFilter>();

            });
        }
 
    }
}
