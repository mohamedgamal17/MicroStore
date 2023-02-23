using Duende.IdentityServer.EntityFramework.Entities;
using IdentityModel;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using MicroStore.IdentityProvider.IdentityServer.Infrastructure.Services;
using static IdentityModel.OidcConstants;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.Clients
{
    public class ClientCommandServiceBaseTest : BaseTestFixture
    {
        public const string SecretType = "SharedSecret";
        protected ClientModel PrepareClientModel()
        {
            return new ClientModel
            {
                ClientId = Guid.NewGuid().ToString(),
                ClientName = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                LogoUri = "http://exmple.com/fake.pnj",
                ClientUri = "http://exmple.com/",
                RedirectUris = new List<string>
                {
                    "http://exmple.com/"
                },
                PostLogoutRedirectUris = new List<string>
                {
                     "http://exmple.com/"
                },

                AllowedCorsOrigins = new List<string>
                {
                      "http://exmple.com/"
                },
                RequirePkce = true,
                RequireClientSecret = true,
                AllowedGrantTypes = new List<string>
                {
                    GrantTypes.ClientCredentials
                },

                AllowedScopes = new List<string>
                {
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString()
                },

                AllowOfflineAccess = true,
                AllowRememberConsent = true,
                RequireConsent = true,
                ConsentLifetime = 5412,
                BackChannelLogoutSessionRequired = true,
                BackChannelLogoutUri = "http://exmple.com/",
                FrontChannelLogoutSessionRequired = true,
                FrontChannelLogoutUri = "http://exmple.com/",
                AccessTokenLifetime = 54121,
                AccessTokenType = Duende.IdentityServer.Models.AccessTokenType.Jwt,
                IdentityTokenLifetime = 5412,
                AlwaysIncludeUserClaimsInIdToken = true,
                RefreshTokenExpiration = Duende.IdentityServer.Models.TokenExpiration.Absolute,
                RefreshTokenUsage = Duende.IdentityServer.Models.TokenUsage.OneTimeOnly,
                UpdateAccessTokenClaimsOnRefresh = true,
                AbsoluteRefreshTokenLifetime = 54231,
                SlidingRefreshTokenLifetime = 3600,

                Properties = new List<PropertyModel>
                {
                    new PropertyModel { Key = Guid.NewGuid().ToString(), Value = Guid.NewGuid().ToString()}
                },
            };
        }


        protected SecretModel PrepareSecretModel()
        {
            return new SecretModel
            {
                Description = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString()
            };
        }

        protected Task<Client> GenerateFakeClient()
        {
            var client = new Client
            {
                ClientName = Guid.NewGuid().ToString(),
                ClientId = Guid.NewGuid().ToString(),
                ClientUri = "http://exmple.com/",
                LogoUri = "http://exmple.com/",
                AllowedGrantTypes = new List<Duende.IdentityServer.EntityFramework.Entities.ClientGrantType>
                {
                    new Duende.IdentityServer.EntityFramework.Entities.ClientGrantType { GrantType = GrantTypes.ClientCredentials },
                },

                RedirectUris = new List<Duende.IdentityServer.EntityFramework.Entities.ClientRedirectUri>
                {
                    new Duende.IdentityServer.EntityFramework.Entities.ClientRedirectUri
                    {
                        RedirectUri = "http://exmple.com/"
                    }
                },

                RequireConsent = false,
                AllowOfflineAccess = true,
                AccessTokenLifetime = 60,
                AllowedScopes = new List<Duende.IdentityServer.EntityFramework.Entities.ClientScope>
                {
                    new Duende.IdentityServer.EntityFramework.Entities.ClientScope
                    {
                        Scope = Guid.NewGuid().ToString()
                    }
                }
            };

            return Insert(client);
        }

        protected async Task<Client> GenerateFakeClientWithSecret()
        {
            var client = await GenerateFakeClient();

            client.ClientSecrets = new List<Duende.IdentityServer.EntityFramework.Entities.ClientSecret>
            {
                new Duende.IdentityServer.EntityFramework.Entities.ClientSecret
                {
                    Value = Guid.NewGuid().ToString().ToSha512(),
                    Type = SecretType,
                    Description = Guid.NewGuid().ToString()
                }
            };


            return await Update(client);
        }
    }


   
}
