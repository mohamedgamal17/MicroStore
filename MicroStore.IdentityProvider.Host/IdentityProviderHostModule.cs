using Duende.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.IdentityProvider.Host.Seeders;
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MicroStore.BuildingBlocks.AspNetCore.Infrastructure;
using Volo.Abp.AutoMapper;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.EntityFramework.Stores;

namespace MicroStore.IdentityProvider.Host
{

    [DependsOn(typeof(IdentityServerInfrastrcutreModule),
        typeof(IdentityInfrastructureModule),
        typeof(MicroStoreAspNetCoreModule),
        typeof(MediatorModule),
        typeof(AbpAutofacModule))]
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

            Configure<AbpAutoMapperOptions>(opt => opt.AddMaps<IdentityApplicationModule>());

        }


        public override async Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                await PrepareDataBaseMigration(scope.ServiceProvider);


                var env = context.GetEnvironment();

                if (env.IsDevelopment())
                {
                    var seedEngine = context.ServiceProvider.GetRequiredService<IDataSeederEngine>();

                    await seedEngine.TryToSeedAsync(scope.ServiceProvider);

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
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API");
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

                    // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
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
                }).AddAspNetIdentity<ApplicationIdentityUser>();


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
