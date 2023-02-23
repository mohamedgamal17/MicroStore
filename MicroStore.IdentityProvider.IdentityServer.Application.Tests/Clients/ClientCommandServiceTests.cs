using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.IdentityServer.Application.Clients;
using MicroStore.IdentityProvider.IdentityServer.Application.Tests.Extensions;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.Clients
{
    public class ClientCommandServiceTests : ClientCommandServiceBaseTest
    {
        private readonly IClientCommandService _clientCommandService;

        public ClientCommandServiceTests()
        {
            _clientCommandService = GetRequiredService<IClientCommandService>();
        }

        [Test]
        public async Task Should_create_client()
        {
            var model = PrepareClientModel();

            var result = await _clientCommandService.CreateAsync(model);

            result.IsSuccess.Should().BeTrue();

            var client = await SingleAsync<Client>(x => x.Id == result.Value.Id);

            client.AssertClient(result.Value);

            client.AssertClientCommand(model);
        }

        [Test]
        public async Task Should_update_client()
        {
            var fakeClient = await GenerateFakeClient();

            var model = PrepareClientModel();

            var result = await _clientCommandService.UpdateAsync(fakeClient.Id, model);

            result.IsSuccess.Should().BeTrue();

            var client = await SingleAsync<Client>(x => x.Id == fakeClient.Id);

            client.AssertClient(result.Value);

            client.AssertClientCommand(model);
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_client_when_client_is_not_exist()
        {
            var model = PrepareClientModel();

            var result = await _clientCommandService.UpdateAsync(int.MaxValue, model);

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }

        [Test]
        public async Task Should_remove_client()
        {
            var fakeClient = await GenerateFakeClient();

            var result = await _clientCommandService.DeleteAsync(fakeClient.Id);

            result.IsSuccess.Should().BeTrue();
        }


        [Test]
        public async Task Should_return_failure_result_while_removing_client_when_client_is_not_exist()
        {
            var result = await _clientCommandService.DeleteAsync(int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }

        [Test]
        public async Task Should_create_client_secret()
        {
            var fakeClient = await GenerateFakeClient();

            var model = PrepareSecretModel();

            var result = await _clientCommandService.AddClientSecret(fakeClient.Id, model);

            result.IsSuccess.Should().BeTrue();

            var client = await SingleAsync<Client>(x => x.Id == fakeClient.Id);

            client.ClientSecrets.Count.Should().Be(1);

            var clientSecret = client.ClientSecrets.First();

            clientSecret.Type.Should().Be(SecretType);

            clientSecret.Description.Should().Be(model.Description);
        }


        [Test]
        public async Task Should_return_failure_while_creating_client_secret_when_client_is_not_exist()
        {
            var model = PrepareSecretModel();

            var result = await _clientCommandService.AddClientSecret(int.MaxValue, model);

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }


        [Test]
        public async Task should_remove_client_secret()
        {
            var fakeClient = await GenerateFakeClientWithSecret();

            var result = await _clientCommandService.DeleteClientSecret(fakeClient.Id,
                fakeClient.ClientSecrets.First().Id);

            result.IsSuccess.Should().BeTrue();

            var client = await SingleAsync<Client>(x => x.Id == fakeClient.Id);

            client.ClientSecrets.Count.Should().Be(0);
        }

        [Test]
        public async Task Should_return_failure_result_while_removing_client_secret_when_client_is_not_exist()
        {
       
            var result = await _clientCommandService.DeleteClientSecret(int.MaxValue, int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }

        [Test]
        public async Task Should_return_failure_result_while_removing_client_secret_when_client_is_not_contain_specific_secret()
        {
            var fakeClient = await GenerateFakeClient();

            var result = await _clientCommandService.DeleteClientSecret(fakeClient.Id, int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);

        }

    }

}
