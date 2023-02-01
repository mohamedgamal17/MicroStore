using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using MicroStore.IdentityProvider.IdentityServer.Application.Clients.Commands;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Models;
using MicroStore.IdentityProvider.IdentityServer.Application.Tests.Extensions;
using System.Net;
using static IdentityModel.OidcConstants;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.Clients
{
    public class When_receving_create_client_command : BaseTestFixture
    {
        [Test]
        public async Task Should_create_client()
        {
            var command = GenerateFakeClientCommand();

            var response = await Send(command);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.Created);

            var client = await SingleAsync<Client>(x => x.Id == response.EnvelopeResult.Result.Id);

            client.AssertClient(response.EnvelopeResult.Result);

            client.AssertClientCommand(command);
        }


        private CreateClientCommand GenerateFakeClientCommand()
        {
            return new CreateClientCommand
            {
                ClientName = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                LogoUri = "http://exmple.com/fake.pnj",
                ClientUri = "http://exmple.com/",
                AllowedRedirectUris = new List<string>
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
    }

    public class When_receving_update_client_command : BaseTestFixture
    {
        [Test]
        public async Task Should_update_client()
        {
            var fakeClient = await GenerateFakeClient();

            var command = GenerateFakeClientCommand();

            command.ClientId = fakeClient.Id;

            var response = await Send(command);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var client=  await SingleAsync<Client>(x=> x.Id == fakeClient.Id);

            client.AssertClient(response.EnvelopeResult.Result);

            client.AssertClientCommand(command);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_while_client_is_not_exist()
        {
            var command = GenerateFakeClientCommand();

            command.ClientId = int.MaxValue;

            var response = await Send(command);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        private Task<Client> GenerateFakeClient()
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
        private UpdateClientCommand GenerateFakeClientCommand()
        {
            return new UpdateClientCommand
            {
                ClientName = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                LogoUri = "http://exmple.com/fake.pnj",
                ClientUri = "http://exmple.com/",
                AllowedRedirectUris = new List<string>
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
    }


    public class When_receving_remove_client_command : BaseTestFixture
    {
        [Test]
        public async Task Should_remove_client()
        {

            var fakeClient=  await GenerateFakeClient();

            var command = new RemoveClientCommand { ClinetId = fakeClient.Id };

            var response = await Send(command);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int) HttpStatusCode.NoContent);
        }


        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_while_client_is_not_exist()
        {
            var command = new RemoveClientCommand { ClinetId = int.MaxValue };

            command.ClinetId = int.MaxValue;

            var response = await Send(command);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        private Task<Client> GenerateFakeClient()
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
    }



}
