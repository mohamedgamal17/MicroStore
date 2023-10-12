using Hellang.Middleware.ProblemDetails;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MicroStore.Payment.Api.Host.OpenApi;
using MicroStore.Payment.Application.Configuration;
using MicroStore.Payment.Application.EntityFramework;
using MicroStore.Payment.Application.Security;
using MicroStore.Payment.Plugin.StripeGateway;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IdentityModel.Tokens.Jwt;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
namespace MicroStore.Payment.Api.Host
{

    [DependsOn(typeof(PaymentApiModule),
        typeof(StripeGatewayPluginModule),
        typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreSerilogModule))]
    public class PaymentApiHostModule : AbpModule
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

            context.Services.AddScoped<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>()
                .AddSwaggerGen();
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
                options.AddPolicy(ApplicationPolicies.RequireAuthenticatedUser,
                    policyBuilder => policyBuilder.RequireAuthenticatedUser()
                        .RequireClaim(JwtClaimTypes.Scope , ApplicationResourceScopes.Access)
                        .Build()
                 );

                options.AddPolicy(ApplicationPolicies.RequirePaymentReadScope,
                    policyBuilder => policyBuilder.RequireAuthenticatedUser()
                        .RequireClaim(JwtClaimTypes.Scope, ApplicationResourceScopes.Access)
                        .RequireClaim(JwtClaimTypes.Scope, ApplicationResourceScopes.Payment.Read)
                        .Build()
                 );

                options.AddPolicy(ApplicationPolicies.RequirePaymentWriteScope,
                    policyBuilder => policyBuilder.RequireAuthenticatedUser()
                        .RequireClaim(JwtClaimTypes.Scope, ApplicationResourceScopes.Access)
                        .RequireClaim(JwtClaimTypes.Scope, ApplicationResourceScopes.Payment.Write)
                        .Build()
                 );

            });
        }

        public override async Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            using var scope = context.ServiceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();

            await dbContext.Database.MigrateAsync();

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
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment API");
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

    }
}
