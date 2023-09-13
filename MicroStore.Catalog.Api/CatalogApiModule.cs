using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.Catalog.Api.Infrastructure;
using MicroStore.Catalog.Api.OpenApi;
using MicroStore.Catalog.Application;
using MicroStore.Catalog.Domain.Configuration;
using MicroStore.Catalog.Infrastructure;
using MicroStore.Catalog.Infrastructure.EntityFramework;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Serialization;
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
            var env = context.Services.GetHostingEnvironment();

            var configuration = context.Services.GetConfiguration();

            context.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>()
                .AddSwaggerGen()
                .AddHttpClient();
                

            ConfigureAuthentication(context.Services, configuration);

            Configure<AbpExceptionHandlingOptions>(options =>
            {
                options.SendExceptionsDetailsToClients = false;
                options.SendStackTraceToClients = false;
            });

            Configure<AbpAntiForgeryOptions>(opt =>
            {
                opt.AutoValidate = false;
            });

            context.Services.AddProblemDetails(opt =>
            {
                opt.IncludeExceptionDetails = (ctx, ex) => env.IsDevelopment() || env.IsStaging();
                opt.ShouldLogUnhandledException = (ctx, ex, proplemDetails) => true;
                opt.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
                opt.MapToStatusCode<HttpRequestException>(StatusCodes.Status502BadGateway);
         
            });


            Configure<JsonOptions>(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        }


        private void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {

            var authority = configuration.GetValue<string>("Security:Jwt:Authority");
            var audience = configuration.GetValue<string>("Security:Jwt:Audience");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.Audience = audience;
                options.MapInboundClaims = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authority,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    
                };
            });

            var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
               .RequireAuthenticatedUser()
               .Build();


            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy(ApplicationAuthorizationPolicy.RequeireAuthenticatedUser, (policy) =>
                    policy.RequireAuthenticatedUser());
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
            var appSettings = context.ServiceProvider.GetRequiredService<ApplicationSettings>();

            if (env.IsDevelopment())
            {

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.OAuthClientId(appSettings.Security.SwaggerClient.ClientId);
                    options.OAuthClientSecret(appSettings.Security.SwaggerClient.ClientSecret);
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API");
                    options.OAuthScopeSeparator(" ");
                    options.OAuthUsePkce();
                });

                app.UseHsts();

            }
        

            app.UseProblemDetails();
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
    }

}
