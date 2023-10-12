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
using Hellang.Middleware.ProblemDetails;
using Microsoft.IdentityModel.Tokens;
using MicroStore.Inventory.Domain.Configuration;
using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using MicroStore.Geographic.Host.OpenApi;

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

            context.Services.AddProblemDetails(opt =>
            {
                opt.IncludeExceptionDetails = (ctx, ex) => env.IsDevelopment() || env.IsStaging();
                opt.ShouldLogUnhandledException = (ctx, ex, proplemDetails) => true;
                opt.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
                opt.MapToStatusCode<HttpRequestException>(StatusCodes.Status502BadGateway);

            });

            context.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>()
                .AddSwaggerGen();

        }
        public override async Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            using var scope = context.ServiceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

            await dbContext.Database.MigrateAsync();

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

    }
}
