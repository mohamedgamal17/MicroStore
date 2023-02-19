using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Infrastructure;
using MicroStore.ShoppingCart.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace MicroStore.ShoppingCart.Api
{
    [DependsOn(typeof(MicroStoreAspNetCoreModule),
            typeof(AbpAutofacModule),
            typeof(AbpAspNetCoreSerilogModule),
            typeof(AbpCachingStackExchangeRedisModule),
            typeof(AbpAutoMapperModule))]
    public class ShoppingCartApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var host = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddProfile<BasketProfile>();
            });

            Configure<AbpAntiForgeryOptions>(opt =>
            {
                opt.AutoValidate = false;
            });

            ConfigureAuthentication(context.Services, configuration);

            ConfigureSwagger(context.Services,configuration);
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
            });
        }

        private void ConfigureSwagger(IServiceCollection services , IConfiguration configuration)
        {
            services.AddSwaggerGen((options) =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket Api", Version = "v1" });
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
                            
                        },
                        
                    }
                    

                    
                });
            });
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
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API");
                    options.OAuthClientId(config.GetValue<string>("SwaggerClinet:Id"));
                    options.OAuthClientSecret(config.GetValue<string>("SwaggerClinet:Secret"));
                    options.UseRequestInterceptor("(req) => { if (req.url.endsWith('oauth/token') && req.body) req.body += '&audience=" + config.GetValue<string>("IdentityProvider:Audience") + "'; return req; }");
                    options.OAuthScopeSeparator(",");

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
        }

       
    }
}
