using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MicroStore.Gateway.Shopping.Config;
using MicroStore.Gateway.Shopping.Services;
using MicroStore.Gateway.Shopping.TokenHandlers;
using Ocelot.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace MicroStore.Gateway.Shopping.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection ConfigureCoreServices(this IServiceCollection services, IConfiguration configuration)
        {


            services.Configure<GatewayClientOptions>(configuration.GetSection("GatewayClient"));

            services.Configure<IdentityProviderOptions>(configuration.GetSection("IdentityProvider"));

            services.AddHttpContextAccessor();

            services.AddHttpClient();

            services.AddDistributedMemoryCache();

            services.AddAccessTokenManagement();

            services.AddSingleton<HttpContextClaimsPrincibalAccessor>();

            ConfigureAuthentication(services, configuration);

            ConfigureOcelot(services);

            return services;
        }


        private static IServiceCollection ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
                {
                    opt.Authority = configuration.GetValue<string>("IdentityProvider:Authority");
                    opt.Audience = configuration.GetValue<string>("IdentityProvider:Audience");
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = configuration.GetValue<string>("IdentityProvider:Audience"),
                        ValidateIssuer = true,
                        ValidIssuer = configuration.GetValue<string>("IdentityProvider:Authority"),
                        ValidateLifetime = true,

                    };

                });

            return services;
        }

        private static IServiceCollection ConfigureOcelot(IServiceCollection services)
        {
            services.AddOcelot()
                .AddDelegatingHandler<DefaultTokenExchangeDelegatingHandler>();

            return services;
        }


    }
}
