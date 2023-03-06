using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Infrastructure;
using MicroStore.Catalog.Application;
using MicroStore.Catalog.Infrastructure;
using MicroStore.Catalog.Infrastructure.EntityFramework;
using System.IdentityModel.Tokens.Jwt;
using Volo.Abp;
using Volo.Abp.Application;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
namespace MicroStore.Catalog.Api
{
    [DependsOn(typeof(CatalogApplicationModule),
        typeof(CatalogInfrastructureModule))]
    [DependsOn(typeof(MicroStoreAspNetCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpDddApplicationModule))]
    public class CatalogApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var host = context.Services.GetHostingEnvironment();

            var configuration = context.Services.GetConfiguration();

            ConfigureAuthentication(context.Services, configuration);

            ConfigureSwagger(context.Services, configuration);

            Configure<AbpExceptionHandlingOptions>(options =>
            {
                options.SendExceptionsDetailsToClients = false;
                options.SendStackTraceToClients = false;
            });

            Configure<AbpAntiForgeryOptions>(opt =>
            {
                opt.AutoValidate = false;
            });


        }


        private void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = configuration.GetValue<string>("IdentityProvider:Authority");
                options.Audience = configuration.GetValue<string>("IdentityProvider:Audience");
                options.MapInboundClaims = true;

            });

        }

        public override async Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            using var scope = context.ServiceProvider.CreateScope();

            var dbContext =  scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

            await dbContext.Database.MigrateAsync();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            var config = context.GetConfiguration();

            if (env.IsDevelopment())
            {

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.OAuthClientId(config.GetValue<string>("SwaggerClinet:Id"));
                    options.OAuthClientSecret(config.GetValue<string>("SwaggerClinet:Secret"));
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API");
                    options.UseRequestInterceptor("(req) => { if (req.url.endsWith('oauth/token') && req.body) req.body += '&audience=" + config.GetValue<string>("IdentityProvider:Audience") + "'; return req; }");
                });


                app.UseHsts();

            }


            app.UseAbpRequestLocalization();
            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
            //app.MapControllers();
        }





        private void ConfigureSwagger(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSwaggerGen((options) =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog Api", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                options.OperationFilter<AuthorizeCheckOperationFilter>();
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,

                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(configuration.GetValue<string>("IdentityProvider:Authority")),
                            TokenUrl = new Uri(configuration.GetValue<string>("IdentityProvider:TokenEndpoint")),
                        }
                    }
                });

            });
        }

    }
}
