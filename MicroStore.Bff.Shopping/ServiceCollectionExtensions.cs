using MicroStore.Bff.Shopping.Config;
using MicroStore.Bff.Shopping.Services.Geographic;
using MicroStore.Bff.Shopping.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MicroStore.Bff.Shopping.Helpers;
using MicroStore.Bff.Shopping.Infrastructure;
using Microsoft.OpenApi.Models;
using MicroStore.Bff.Shopping.Services.Profiling;
namespace MicroStore.Bff.Shopping
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBffShopping(this IServiceCollection services, IConfiguration config)
        {
            AddWebServices(services);

            RegisterGrpc(services, config);

            ConfigureAuthentication(services, config);

            ConfigureSwagger(services, config);

            RegisterApplicationService(services);
           
            return services;
        }


        private static void AddWebServices(IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(ExceptionHandlerFilter));
            });

            services.AddControllers();

            services.AddHttpContextAccessor();

            services.AddEndpointsApiExplorer();
        }
        private static void RegisterGrpc(IServiceCollection services, IConfiguration config)
        {
            var grpcConfig = new GrpcConfiguration();

            config.Bind("Grpc",grpcConfig);

            services.AddSingleton(grpcConfig);

            services.AddSingleton<GrpcClientTokenInterceptor>();

            services.AddGrpcClient<Grpc.Geographic.CountryService.CountryServiceClient>(opt =>
            {
                opt.Address = new Uri(grpcConfig.Geographic);
            }).AddInterceptor<GrpcClientTokenInterceptor>();

            services.AddGrpcClient<Grpc.Geographic.StateProvinceService.StateProvinceServiceClient>(opt =>
            {
                opt.Address = new Uri(grpcConfig.Geographic);

            }).AddInterceptor<GrpcClientTokenInterceptor>();

            services.AddGrpcClient<Grpc.Profiling.ProfileService.ProfileServiceClient>(opt =>
            {
                opt.Address = new Uri(grpcConfig.Profiling);

            }).AddInterceptor<GrpcClientTokenInterceptor>();

        }

        public static void ConfigureAuthentication(IServiceCollection services , IConfiguration config)
        {
            var securityConfiguration = new SecurityConfiguration();

            config.Bind("Security", securityConfiguration);

            services.AddSingleton(securityConfiguration);

            var gatewayClientConfig = new GatewayClientConfiguration();

            config.Bind("GatewayClient", gatewayClientConfig);

            services.AddSingleton(gatewayClientConfig);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.Authority = securityConfiguration.Jwt.Authority;
                opt.Audience = securityConfiguration.Jwt.Audience;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = securityConfiguration.Jwt.Audience,
                    ValidateIssuer = true,
                    ValidIssuer = securityConfiguration.Jwt.Authority,
                    ValidateLifetime = true,
                    ValidTypes = new[] { "at+jwt" },
                };

                opt.ForwardDefaultSelector = Selector.ForwardReferenceToken();
            }).AddOAuth2Introspection("introspection", opt =>
            {
                opt.Authority = securityConfiguration.Jwt.Authority;

                opt.ClientId = securityConfiguration.Jwt.Audience;
                opt.ClientSecret = securityConfiguration.Jwt.Secret;
                opt.SaveToken = true;
                opt.ClaimsIssuer = securityConfiguration.Jwt.Authority;
                opt.EnableCaching = true;
            });

            services.AddTransient<ITokenService, TokenService>();

            services.AddTransient<ApplicationCurrentUser>();

            services.AddAccessTokenManagement();
        }

        private static void ConfigureSwagger(IServiceCollection services,  IConfiguration config)
        {
            var securityConfiguration = new SecurityConfiguration();

            config.Bind("Security", securityConfiguration);

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Shopping Aggregator Gateway",
                    Version ="v1",
                    
                });
                opt.CustomSchemaIds(x => x.FullName);
                opt.DocInclusionPredicate((docName, description) => true);
                opt.OperationFilter<AuthorizeCheckOperationFilter>();
                if(securityConfiguration.SwaggerClient != null)
                {
                    opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,

                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri(securityConfiguration.Jwt.AuthorizationEndPoint),
                                TokenUrl = new Uri(securityConfiguration.Jwt.TokenEndPoint),
                                Scopes = securityConfiguration.SwaggerClient.Scopes
                                
                            },

                        },
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        Description = "OpenId Security Scheme"
                    });
                }  
            });
        }
        private static void RegisterApplicationService(IServiceCollection services)
        {
            services.AddTransient<CountryService>();

            services.AddTransient<StateProvinceService>();

            services.AddTransient<ProfilingService>();
        }
    }
}
