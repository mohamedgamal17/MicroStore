using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MicroStore.BuildingBlocks.AspNetCore.Infrastructure;
using MicroStore.Shipping.Domain.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace MicroStore.Shipping.Host.OpenApi
{
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {

        private readonly ApplicationSettings _applicationSettings;

        public ConfigureSwaggerGenOptions(ApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }


        private OpenApiInfo PrepareOpenApiInfo()
        {
            var openApiInfo = new OpenApiInfo
            {
                Title = "Shipping Api",
                Version = "v1",
                Description = "Shipping api ",

            };

            return openApiInfo;
        }
        public void Configure(SwaggerGenOptions options)
        {
            options.OperationFilter<AuthorizeCheckOperationFilter>();
            options.OperationFilter<SnakeCaseParamsOperationFilter>();
            options.CustomSchemaIds(x => x.FullName);
            options.DocInclusionPredicate((docName, description) => true);
            options.SwaggerDoc("v1", PrepareOpenApiInfo());

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,

                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(_applicationSettings.Security.Jwt.AuthorizationEndPoint),
                        TokenUrl = new Uri(_applicationSettings.Security.Jwt.TokenEndPoint),
                        Scopes = _applicationSettings.Security.SwaggerClient.Scopes

                    },

                },
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "OpenId Security Scheme"
            });

        }
    }
}
