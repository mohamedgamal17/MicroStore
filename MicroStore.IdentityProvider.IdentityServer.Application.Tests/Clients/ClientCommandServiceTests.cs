using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using MicroStore.IdentityProvider.IdentityServer.Application.Clients;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using MicroStore.IdentityProvider.IdentityServer.Application.Tests.Extensions;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

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

            client.AssertClientModel(model);
        }

        [Test]
        public async Task Should_update_client()
        {
            var fakeClient = await GenerateFakeClient();

            var model = PrepareClientModel();

            foreach (var scope in fakeClient.AllowedScopes)
            {
                model.AllowedScopes!.Add(scope.Scope);
            }

            var result = await _clientCommandService.UpdateAsync(fakeClient.Id, model);

            result.IsSuccess.Should().BeTrue();

            var client = await SingleAsync<Client>(x => x.Id == fakeClient.Id);

            client.AssertClient(result.Value);

            client.AssertClientModel(model);
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_client_when_client_is_not_exist()
        {
            var model = PrepareClientModel();

            var result = await _clientCommandService.UpdateAsync(int.MaxValue, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
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

            result.Exception.Should().BeOfType<EntityNotFoundException>();
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

            result.Exception.Should().BeOfType<EntityNotFoundException>();
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

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_while_removing_client_secret_when_client_is_not_contain_specific_secret()
        {
            var fakeClient = await GenerateFakeClient();

            var result = await _clientCommandService.DeleteClientSecret(fakeClient.Id, int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_add_new_client_claim()
        {
            var fakeClient = await GenerateFakeClient();

            var model = new ClaimModel
            {
                Type = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _clientCommandService.AddClaim(fakeClient.Id, model);

            result.IsSuccess.Should().BeTrue();

            var client = await SingleAsync<Client>(x => x.Id == fakeClient.Id);

            var property = client.Claims.Single(x => x.Type == model.Type);

            property.Type.Should().Be(model.Type);

            property.Value.Should().Be(model.Value);

            client.Claims.Count.Should().Be(fakeClient.Claims.Count + 1);
        }

        [Test]
        public async Task Should_return_failure_result_while_adding_claim_when_client_is_not_found()
        {
            var model = new ClaimModel
            {
                Type = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _clientCommandService.AddClaim(int.MaxValue, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_while_adding_claim_when_claim_type_and_value_is_already_exist()
        {
            var fakeClient = await GenerateFakeClient();
            var fakeClaim = fakeClient.Claims.First();

            var model = new ClaimModel
            {
                Type = fakeClaim.Type,
                Value =fakeClaim.Value,
            };

            var result = await _clientCommandService.AddClaim(fakeClient.Id, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<UserFriendlyException>();
        }


        [Test]
        public async Task Should_update_client_claim()
        {
            var fakeClient = await GenerateFakeClient();

            var fakeClaim = fakeClient.Claims.First();

            var model = new ClaimModel
            {
                Type = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _clientCommandService.UpdateClaim(fakeClient.Id, fakeClaim.Id, model);

            result.IsSuccess.Should().BeTrue();

            var client = await SingleAsync<Client>(x => x.Id == fakeClient.Id);

            var property = client.Claims.Single(x => x.Id == fakeClaim.Id);

            property.Type.Should().Be(model.Type);

            property.Value.Should().Be(model.Value);
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_claim_when_client_is_not_found()
        {
            var model = new ClaimModel
            {
                Type = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _clientCommandService.UpdateClaim(int.MaxValue, int.MaxValue ,model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }  
        
        [Test]
        public async Task Should_return_failure_result_while_updating_claim_when_claim_is_not_found()
        {
            var fakeClient = await GenerateFakeClient();

            var model = new ClaimModel
            {
                Type = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _clientCommandService.UpdateClaim(fakeClient.Id, int.MaxValue ,model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_claim_when_claim_type_and_value_is_already_exist()
        {
            var fakeClient = await GenerateFakeClient();
            var fakeFirstClaim = fakeClient.Claims.First();
            var fakeLastClaim = fakeClient.Claims.Last();

            var model = new ClaimModel
            {
                Type = fakeLastClaim.Type,
                Value = fakeLastClaim.Value,
            };

            var result = await _clientCommandService.UpdateClaim(fakeClient.Id, fakeFirstClaim.Id, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<UserFriendlyException>();
        }

        [Test]
        public async Task Should_remove_client_claim()
        {
            var fakeClient = await GenerateFakeClient();

            var fakeClaim = fakeClient.Claims.First();


            var result = await _clientCommandService.RemoveClaim(fakeClient.Id, fakeClaim.Id);

            result.IsSuccess.Should().BeTrue();

            var client = await SingleAsync<Client>(x => x.Id == fakeClient.Id);

            client.Claims.Count.Should().Be(fakeClient.Claims.Count - 1);
        }

        [Test]
        public async Task Should_return_failure_result_while_removing_claim_when_client_is_not_found()
        {
            var model = new ClaimModel
            {
                Type = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _clientCommandService.RemoveClaim(int.MaxValue, int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_while_Removing_claim_when_claim_is_not_found()
        {
            var fakeClient = await GenerateFakeClient();

            var model = new ClaimModel
            {
                Type = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _clientCommandService.RemoveClaim(fakeClient.Id, int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        [Test]
        public async Task Should_add_new_client_properties()
        {
            var fakeClient = await GenerateFakeClient();

            var model = new PropertyModel
            {
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _clientCommandService.AddProperty(fakeClient.Id, model);

            result.IsSuccess.Should().BeTrue();

            var client = await SingleAsync<Client>(x => x.Id == fakeClient.Id);

            var property = client.Properties.Single(x => x.Key == model.Key);

            property.Key.Should().Be(model.Key);

            property.Value.Should().Be(model.Value);

            client.Properties.Count.Should().Be(fakeClient.Properties.Count + 1);

        }

        [Test]
        public async Task Should_return_failure_result_while_adding_property_when_client_is_not_found()
        {
            var model = new PropertyModel
            {
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _clientCommandService.AddProperty(int.MaxValue, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        [Test]
        public async Task Should_return_failure_result_while_adding_property_when_property_key_is_already_exist()
        {
            var fakeClient = await GenerateFakeClient();
            var fakeProperty = fakeClient.Properties.First();

            var model = new PropertyModel
            {
                Key = fakeProperty.Key,
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _clientCommandService.AddProperty(fakeClient.Id, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<UserFriendlyException>();
        }

        [Test]
        public async Task Should_update_client_property()
        {
            var fakeClient = await GenerateFakeClient();

            var fakeProperty = fakeClient.Properties.First();

            var model = new PropertyModel
            {
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _clientCommandService.UpdateProperty(fakeClient.Id, fakeProperty.Id, model);

            result.IsSuccess.Should().BeTrue();

            var client = await SingleAsync<Client>(x => x.Id == fakeClient.Id);

            var property = client.Properties.Single(x => x.Id == fakeProperty.Id);

            property.Key.Should().Be(model.Key);

            property.Value.Should().Be(model.Value);

        }


        [Test]
        public async Task Should_return_failure_result_while_updating_property_when_client_is_not_found()
        {
            var model = new PropertyModel
            {
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _clientCommandService.UpdateProperty(int.MaxValue, int.MaxValue, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_property_when_client_property_is_not_found()
        {
            var fakeClient = await GenerateFakeClient();

            var model = new PropertyModel
            {
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _clientCommandService.UpdateProperty(fakeClient.Id, int.MaxValue, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_property_when_property_key_is_already_exist()
        {
            var fakeClient = await GenerateFakeClient();
            var fakeFirstProperty = fakeClient.Properties.First();
            var fakeLastProperty = fakeClient.Properties.Last();

            var model = new PropertyModel
            {
                Key = fakeLastProperty.Key,
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _clientCommandService.UpdateProperty(fakeClient.Id, fakeFirstProperty.Id, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<UserFriendlyException>();
        }


        [Test]
        public async Task Should_remove_client_property()
        {
            var fakeClient = await GenerateFakeClient();

            var fakeProperty = fakeClient.Properties.First();

            var result = await _clientCommandService.RemoveProperty(fakeClient.Id, fakeProperty.Id);

            result.IsSuccess.Should().BeTrue();

            var client = await SingleAsync<Client>(x => x.Id == fakeClient.Id);

            client.Properties.Count.Should().Be(fakeClient.Properties.Count - 1);
        }

        [Test]
        public async Task Should_return_failure_result_while_removing_client_property_when_client_not_exist()
        {
            var result = await _clientCommandService.RemoveProperty(int.MaxValue, int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_return_failure_result_while_removing_client_property_when_client_property_not_exist()
        {
            var fakeClient = await GenerateFakeClient();

            var result = await _clientCommandService.RemoveProperty(fakeClient.Id, int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

    }

}
