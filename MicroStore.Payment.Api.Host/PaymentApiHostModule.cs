using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MicroStore.BuildingBlocks.AspNetCore.Infrastructure;
using MicroStore.Payment.Domain.Security;
using MicroStore.Payment.Plugin.StripeGateway;
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
            var host = context.Services.GetHostingEnvironment();

            var configuration = context.Services.GetConfiguration();

            ConfigureAuthentication(context.Services, configuration);

            ConfigureSwagger(context.Services,configuration);

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
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment API");
                    options.OAuthClientId(config.GetValue<string>("SwaggerClinet:Id"));
                    options.OAuthClientSecret(config.GetValue<string>("SwaggerClinet:Secret"));
                    options.UseRequestInterceptor("(req) => { if (req.url.endsWith('oauth/token') && req.body) req.body += '&audience=" + config.GetValue<string>("IdentityProvider:Audience") + "'; return req; }");
                    options.OAuthScopeSeparator(",");

                    options.OAuthScopes(BillingScope.List().ToArray());

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





        private void ConfigureSwagger(IServiceCollection serviceCollection , IConfiguration configuration)
        {
            serviceCollection.AddSwaggerGen((options) =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment Api", Version = "v1" });
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

    }
}
