using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp;
using Volo.Abp.Modularity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using MicroStore.Shipping.Plugin.ShipEngineGateway;
using MicroStore.Shipping.WebApi;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Data;
using MicroStore.Shipping.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.IdentityModel.Tokens;
using MicroStore.Shipping.Domain.Configuration;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using MicroStore.Shipping.Host.OpenApi;
using MicroStore.Shipping.Domain.Security;
using IdentityModel;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace MicroStore.Shipping.Host
{
    [DependsOn(typeof(ShippingApiModule),
        typeof(ShipEngineSystemModule),
        typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class ShippingHostModule : AbpModule
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

            Configure<JsonOptions>(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            var appsettings = services.GetSingletonInstance<ApplicationSettings>();

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
            services.AddAuthorization(opt =>
            {

                opt.AddPolicy(ApplicationPolicies.RequireAuthenticatedUser,
                        policyBuilder => policyBuilder.RequireAuthenticatedUser()
                        .RequireClaim(JwtClaimTypes.Scope, ApplicationResourceScopes.Access)
                    );

                opt.AddPolicy(ApplicationPolicies.RequireShippingReadScope,
                        policyBuilder => policyBuilder.RequireAuthenticatedUser()
                        .RequireClaim(JwtClaimTypes.Scope, ApplicationResourceScopes.Access)
                        .RequireClaim(JwtClaimTypes.Scope, ApplicationResourceScopes.Shipment.Read)
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
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Shipping API");
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
        }

        public override async Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            using var scope = context.ServiceProvider.CreateScope();

            var dbContext =  scope.ServiceProvider.GetRequiredService<ShippingDbContext>();

            await dbContext.Database.MigrateAsync();

        }


        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            using var scope = context.ServiceProvider.CreateScope();

            var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();

            await  dataSeeder.SeedAsync(new DataSeedContext());

            await base.OnApplicationInitializationAsync(context);

        }

    }
}
