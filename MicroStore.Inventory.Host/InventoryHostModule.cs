using Microsoft.OpenApi.Models;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Modularity;
using Volo.Abp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MicroStore.Inventory.Api;
using Volo.Abp.Autofac;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AspNetCore.Mvc;
using MicroStore.Inventory.Domain.Security;
using MicroStore.BuildingBlocks.AspNetCore.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using MicroStore.Inventory.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace MicroStore.Inventory.Host
{
    [DependsOn(typeof(InventoryApiModule),
        typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class InventoryHostModule :AbpModule
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
        public override async Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            using var scope = context.ServiceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

            await dbContext.Database.MigrateAsync();

        }
        private void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = configuration.GetValue<string>("IdentityProvider:Authority");
                options.Audience = configuration.GetValue<string>("IdentityProvider:Audience");
            });

            services.AddAuthorization();
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
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory API");
                    options.OAuthClientId(config.GetValue<string>("SwaggerClinet:Id"));
                    options.OAuthClientSecret(config.GetValue<string>("SwaggerClinet:Secret"));
                    options.UseRequestInterceptor("(req) => { if (req.url.endsWith('oauth/token') && req.body) req.body += '&audience=" + config.GetValue<string>("IdentityProvider:Audience") + "'; return req; }");
                    options.OAuthScopeSeparator(",");
                    options.OAuthScopes(InventoryScope.List().ToArray());
                });


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
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventory Api", Version = "v1" });
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
                            AuthorizationUrl = new Uri(configuration.GetValue<string>("IdentityProvider:Authority")!),
                            TokenUrl = new Uri(configuration.GetValue<string>("IdentityProvider:TokenEndpoint")!),

                        },

                    }

                });
            });
        }
    }
}
