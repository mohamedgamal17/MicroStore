using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Infrastructure;
using MicroStore.ShoppingCart.Api.Configuration;
using MicroStore.ShoppingCart.Api.Models;
using MicroStore.ShoppingCart.Api.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IdentityModel.Tokens.Jwt;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace MicroStore.ShoppingCart.Api
{
    [DependsOn(typeof(MicroStoreAspNetCoreModule),
            typeof(AbpAutofacModule),
            typeof(AbpAspNetCoreSerilogModule),
            typeof(AbpCachingStackExchangeRedisModule),
            typeof(AbpAutoMapperModule),
           typeof(AbpFluentValidationModule))]
    public class ShoppingCartApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();
            var appSettings = config.Get<ApplicationSettings>();
            context.Services.AddSingleton(appSettings);
        }
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

            context.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>()
            .AddSwaggerGen()
            .AddHttpClient();

            ConfigureAuthentication(context.Services);
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            var appsettings = services.GetSingletonInstance<ApplicationSettings>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = appsettings.Security.Jwt.Authority;
                options.Audience = appsettings.Security.Jwt.Audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = appsettings.Security.Jwt.Audience,
                    ValidateIssuer = true,
                    ValidIssuer = appsettings.Security.Jwt.Authority,
                    ValidateLifetime = true,
                };
            });
        }




        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            var appsettings = context.ServiceProvider.GetRequiredService<ApplicationSettings>();

            var config = context.GetConfiguration();

            if (env.IsDevelopment())
            {

                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API");
                    options.OAuthClientId(appsettings.Security.SwaggerClient.ClientId);
                    options.OAuthClientSecret(appsettings.Security.SwaggerClient.ClientSecret);
                    options.OAuthUsePkce();
                    options.OAuthScopeSeparator(" ");

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
