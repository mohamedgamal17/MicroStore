using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Infrastructure;
using MicroStore.Geographic.Application;
using MicroStore.Geographic.Application.EntityFramework;
using System.IdentityModel.Tokens.Jwt;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Hellang.Middleware.ProblemDetails;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using MicroStore.Geographic.Host.OpenApi;
using MicroStore.Geographic.Application.Configuration;
using MicroStore.Geographic.Application.Security;
using IdentityModel;
namespace MicroStore.Geographic.Host
{
    [DependsOn(typeof(GeographicApplicationModule),
        typeof(AbpAutofacModule),
       typeof(MicroStoreAspNetCoreModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class GeographicHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var env = context.Services.GetHostingEnvironment();

            var configuration = context.Services.GetConfiguration();

            ConfigureAuthentication(context.Services);

            Configure<AbpExceptionHandlingOptions>(options =>
            {
                options.SendExceptionsDetailsToClients = false;
                options.SendStackTraceToClients = false;
            });

            Configure<AbpAntiForgeryOptions>(opt =>
            {
                opt.AutoValidate = false;
            });

            context.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>()
                .AddAbpSwaggerGen();

            context.Services.AddProblemDetails(opt =>
            {
                opt.IncludeExceptionDetails = (ctx, ex) => env.IsDevelopment() || env.IsStaging();
                opt.ShouldLogUnhandledException = (ctx, ex, proplemDetails) => true;
                opt.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
                opt.MapToStatusCode<HttpRequestException>(StatusCodes.Status502BadGateway);

            });

        }
        public override async Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<GeographicDbContext>();

                await dbContext.Database.MigrateAsync();

            }
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            var config = context.GetConfiguration();
            var appsettings = context.ServiceProvider.GetRequiredService<ApplicationSettings>();

            if (env.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory API");
                    options.OAuthClientId(appsettings.Security.SwaggerClient.ClientId);
                    options.OAuthClientSecret(appsettings.Security.SwaggerClient.ClientSecret);
                    options.OAuthScopeSeparator(" ");
                    options.OAuthUsePkce();
                    
                });


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

            services.AddAuthorization(options =>
            {
                options.AddPolicy(ApplicationSecurityPolicies.RequireAuthenticatedUser,
                        policyBuilder => policyBuilder.RequireAuthenticatedUser()
                            .RequireClaim(JwtClaimTypes.Scope, ApplicationResourceScopes.Access)
                    );
            });
        }
    }
}
