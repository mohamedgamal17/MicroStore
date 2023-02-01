using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.IdentityServer.Application.Clients.Commands;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;
using Newtonsoft.Json.Linq;
using System.Net;
using static IdentityModel.OidcConstants;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.Clients
{
    public abstract class ClientSecretCommandTestBase : BaseTestFixture
    {
        public const string SecretType = "SharedSecret";
        public ICryptoServiceProvider CryptoServiceProvider { get; set; }

        public ClientSecretCommandTestBase()
        {
            CryptoServiceProvider = ServiceProvider.GetRequiredService<ICryptoServiceProvider>();
        }
        public Task<Client> GenerateFakeClient()
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

        public async Task<Client> GenerateFakeClientWithSecret()
        {
            var client = await GenerateFakeClient();

            client.ClientSecrets = new List<Duende.IdentityServer.EntityFramework.Entities.ClientSecret>
            {
                new Duende.IdentityServer.EntityFramework.Entities.ClientSecret
                {
                     Value = (await CryptoServiceProvider.GenerateRandomEncodedBase64Key(32)).ToSha512(),
                    Type = SecretType,
                    Description = Guid.NewGuid().ToString()
                }
            };


            return await Update(client);
        }

    }


    public class When_receving_add_client_secret_command : ClientSecretCommandTestBase
    {
        [Test]
        public async Task Should_create_client_secret()
        {
            var fakeClient = await GenerateFakeClient();

            var command = new AddClientSecretCommand
            {
                ClinetId = fakeClient.Id,
                Algoritm = Common.Models.HasingAlgoritm.SHA256,
                Description = Guid.NewGuid().ToString(),
                Value = await CryptoServiceProvider.GenerateRandomEncodedBase64Key(32),
            };


            var response = await Send(command);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var client = await SingleAsync<Client>(x => x.Id == fakeClient.Id);

            client.ClientSecrets.Count.Should().Be(1);

            var clientSecret = client.ClientSecrets.First();

            clientSecret.Type.Should().Be(SecretType);

            clientSecret.Description.Should().Be(command.Description);
        }

        [Test]
        public async Task Should_return_failure_with_status_code_404_notfound_result_while_client_is_not_exist()
        {
            var command = new AddClientSecretCommand
            {
                ClinetId = int.MaxValue,
                Algoritm = Common.Models.HasingAlgoritm.SHA256,
                Description = Guid.NewGuid().ToString(),
                Value = await CryptoServiceProvider.GenerateRandomEncodedBase64Key(32),
            };

            var response = await Send(command);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }


    public class When_receving_remove_client_secret_command : ClientSecretCommandTestBase
    {
        [Test]
        public async Task should_remove_client_secret()
        {
            var fakeClient = await GenerateFakeClientWithSecret();

            var command = new RemoveClientSecretCommand
            {
                ClinetId = fakeClient.Id,
                SecretId = fakeClient.ClientSecrets.First().Id
            };

            var response= await Send(command);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var client = await SingleAsync<Client>(x => x.Id == fakeClient.Id);

            client.ClientSecrets.Count.Should().Be(0);
        }

        [Test]
        public async Task Should_return_failure_with_status_code_404_notfound_result_while_client_is_not_exist()
        {
            var command = new RemoveClientSecretCommand
            {
                ClinetId = int.MaxValue,
                SecretId = int.MaxValue
            };

            var response = await Send(command);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Should_return_failure_with_status_code_400_notfound_result_while_client_is_not_contain_specific_secret()
        {
            var fakeClient = await GenerateFakeClient();

            var command = new RemoveClientSecretCommand
            {
                ClinetId = fakeClient.Id,
                SecretId = int.MaxValue
            };


            var response = await Send(command);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        }
    }
}
