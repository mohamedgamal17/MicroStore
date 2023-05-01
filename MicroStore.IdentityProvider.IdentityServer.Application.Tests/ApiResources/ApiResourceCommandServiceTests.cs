using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using MicroStore.IdentityProvider.IdentityServer.Application.Tests.Extensions;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.ApiResources
{

    public class ApiResourceCommandServiceTests : ApiResourceCommandServiceBaseTest
    {
        private readonly IApiResourceCommandService _apiResourceCommandService;


        public ApiResourceCommandServiceTests()
        {
            _apiResourceCommandService = GetRequiredService<IApiResourceCommandService>();
        }

        [Test]
        public async Task Should_create_api_resource()
        {

            var model = PrepareApiResourceModel();

            var result = await _apiResourceCommandService.CreateAsync(model);

            result.IsSuccess.Should().BeTrue();

            var apiResource = await SingleAsync<ApiResource>(x => x.Id == result.Value.Id);

            apiResource.AssertApiResource(result.Value);

            apiResource.AssertApiResourceModel(model);
        }

        [Test]
        public async Task Should_update_api_resource()
        {
            var fakeApiReesource = await CreateFakeApiResource();

            var model = PrepareApiResourceModel();

            var result = await _apiResourceCommandService.UpdateAsync(fakeApiReesource.Id,model);

            result.IsSuccess.Should().BeTrue();

            var apiResource = await SingleAsync<ApiResource>(x => x.Id == result.Value.Id);

            apiResource.AssertApiResource(result.Value);

            apiResource.AssertApiResourceModel(model);
        }


        [Test]
        public async Task Should_return_failure_result_while_updating_api_resource_when_api_resource_is_not_exist()
        {
            var model = PrepareApiResourceModel();

            var response = await _apiResourceCommandService.UpdateAsync(int.MaxValue, model);

            response.IsFailure.Should().BeTrue();

            response.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        [Test]
        public async Task Should_remove_api_resource()
        {
            var fakeApiReesource = await CreateFakeApiResource();

            var result = await _apiResourceCommandService.DeleteAsync(fakeApiReesource.Id);

            result.IsSuccess.Should().BeTrue();

            var apiResponse = await SingleOrDefaultAsync<ApiResource>(x => x.Id == fakeApiReesource.Id);

            apiResponse.Should().BeNull();

        }


        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_while_api_resource_is_not_exist()
        {
            var result = await _apiResourceCommandService.DeleteAsync(int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        [Test]
        public async Task Should_create_api_resource_secret()
        {

            var fakeApiResource = await CreateFakeApiResource();

            var model = PrepareSecretModel();

            var result = await _apiResourceCommandService.AddSecret(fakeApiResource.Id, model);

            result.IsSuccess.Should().BeTrue();

            var apiResource = await SingleAsync<ApiResource>(x => x.Id == result.Value.Id);

            apiResource.Secrets.Count.Should().Be(1);

            var apiSecret = apiResource.Secrets.First();

            apiSecret.Description.Should().Be(model.Description);

            apiSecret.Type.Should().Be(SecretType);
        }

        [Test]
        public async Task Should_return_failure_result_while_creating_api_resource_secret_when_api_resource_is_not_exist()
        {
            var model = PrepareSecretModel();

            var result = await _apiResourceCommandService.AddSecret(int.MaxValue, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_remove_api_resource_secret()
        {
            var fakeApiResource = await CreateFakeApiResourceWithSecret();

            var result = await _apiResourceCommandService.RemoveSecret(fakeApiResource.Id, fakeApiResource.Secrets.First().Id);

            result.IsSuccess.Should().BeTrue();

            var apiResource = await SingleAsync<ApiResource>(x => x.Id == fakeApiResource.Id);

            apiResource.Secrets.Count.Should().Be(0);

        }

        [Test]
        public async Task Should_return_failure_result_while_removeing_api_resource_secret_when_api_resource_is_not_exist()
        {

            var result = await _apiResourceCommandService.RemoveSecret(int.MaxValue, int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_while_removing_api_resource_secret_when_api_resource_not_contain_specific_secret()
        {
            var fakeApiResource = await CreateFakeApiResourceWithSecret();

            var response = await _apiResourceCommandService.RemoveSecret(fakeApiResource.Id, int.MaxValue);

            response.IsFailure.Should().BeTrue();

            response.Exception.Should().BeOfType<EntityNotFoundException>();
        }






        [Test]
        public async Task Should_add_new_api_resource_properties()
        {
            var fakeApiResource = await CreateFakeApiResource();

            var model = new PropertyModel
            {
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _apiResourceCommandService.AddProperty(fakeApiResource.Id, model);

            result.IsSuccess.Should().BeTrue();

            var apiResource = await SingleAsync<ApiResource>(x => x.Id == fakeApiResource.Id);

            var property = apiResource.Properties.Single(x => x.Key == model.Key);

            property.Key.Should().Be(model.Key);

            property.Value.Should().Be(model.Value);

            apiResource.Properties.Count.Should().Be(fakeApiResource.Properties.Count + 1);

        }

        [Test]
        public async Task Should_return_failure_result_while_adding_property_when_api_scope_is_not_found()
        {
            var model = new PropertyModel
            {
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _apiResourceCommandService.AddProperty(int.MaxValue, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_while_adding_property_when_property_key_is_already_exist()
        {
            var apiResurce = await CreateFakeApiResource();

            var fakeProperty = apiResurce.Properties.First();

            var model = new PropertyModel
            {
                Key = fakeProperty.Key,
                Value = fakeProperty.Value,
            };

            var result = await _apiResourceCommandService.AddProperty(apiResurce.Id, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<UserFriendlyException>();
        }

        [Test]
        public async Task Should_update_api_resource_property()
        {
            var fakeApiResource = await CreateFakeApiResource();

            var fakeProperty = fakeApiResource.Properties.First();

            var model = new PropertyModel
            {
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _apiResourceCommandService.UpdateProperty(fakeApiResource.Id, fakeProperty.Id, model);

            result.IsSuccess.Should().BeTrue();

            var apiResource = await SingleAsync<ApiResource>(x => x.Id == fakeApiResource.Id);

            var property = apiResource.Properties.Single(x => x.Key == model.Key);

            property.Key.Should().Be(model.Key);

            property.Value.Should().Be(model.Value);

        }


        [Test]
        public async Task Should_return_failure_result_while_updating_property_when_api_resource_is_not_found()
        {
            var model = new PropertyModel
            {
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _apiResourceCommandService.UpdateProperty(int.MaxValue, int.MaxValue, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_property_when_api_resource_property_is_not_found()
        {
            var fakeApiResource = await CreateFakeApiResource();

            var model = new PropertyModel
            {
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _apiResourceCommandService.UpdateProperty(fakeApiResource.Id, int.MaxValue, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        [Test]
        public async Task Should_return_failure_result_while_updating_property_when_property_key_is_already_exist()
        {
            var fakeApiResource = await CreateFakeApiResource();

            var firstProperty =  fakeApiResource.Properties.First();
            var lastProperty = fakeApiResource.Properties.Last();

            var model = new PropertyModel
            {
                Key = lastProperty.Key,
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _apiResourceCommandService.UpdateProperty(fakeApiResource.Id, firstProperty.Id, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<UserFriendlyException>();
        }

        [Test]
        public async Task Should_remove_api_resource_property()
        {
            var fakeApiResource = await CreateFakeApiResource();

            var fakeProperty = fakeApiResource.Properties.First();

            var result = await _apiResourceCommandService.RemoveProperty(fakeApiResource.Id, fakeProperty.Id);

            result.IsSuccess.Should().BeTrue();

            var apiResource = await SingleAsync<ApiResource>(x => x.Id == fakeApiResource.Id);

            apiResource.Properties.Count.Should().Be(fakeApiResource.Properties.Count - 1);
        }

        [Test]
        public async Task Should_return_failure_result_while_removing_api_resource_property_when_api_resource_not_exist()
        {
            var result = await _apiResourceCommandService.RemoveProperty(int.MaxValue, int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_return_failure_result_while_removing_api_resource_property_when_api_resource_property_not_exist()
        {
            var fakeApiScope = await CreateFakeApiResource();

            var result = await _apiResourceCommandService.RemoveProperty(fakeApiScope.Id, int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

    }

}
