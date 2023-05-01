using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using MicroStore.IdentityProvider.IdentityServer.Application.Tests.Extensions;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.ApiScopes
{
    public class ApiScopeCommandServiceTests : BaseTestFixture
    {
        private readonly IApiScopeCommandService _apiScopeCommandService;

        public ApiScopeCommandServiceTests()
        {
            _apiScopeCommandService = GetRequiredService<IApiScopeCommandService>();
        }

        [Test]
        public async Task Should_create_api_scope()
        {
            var model = PrepareApiScopeModel();

            var result = await _apiScopeCommandService.CreateAsync(model);

            result.IsSuccess.Should().BeTrue();

            var apiScope = await SingleAsync<ApiScope>(x => x.Id == result.Value.Id);

            apiScope.AssertApiScope(result.Value);

            apiScope.AssertApiScopeModel(model);
        }

        [Test]
        public async Task Should_update_api_scope()
        {
            var fakeApiScope = await GenerateFakeApiScope();

            var model = PrepareApiScopeModel();

            var result = await _apiScopeCommandService.UpdateAsync(fakeApiScope.Id,model);

            result.IsSuccess.Should().BeTrue();

            var apiScope = await SingleAsync<ApiScope>(x => x.Id == result.Value.Id);

            apiScope.AssertApiScope(result.Value);

            apiScope.AssertApiScopeModel(model);
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_api_scope_when_api_scope_is_not_exist()
        {
            var model = PrepareApiScopeModel();

            var result = await _apiScopeCommandService.UpdateAsync(int.MaxValue,model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        [Test]
        public async Task Should_remove_api_scope()
        {
            var fakeApiScope = await GenerateFakeApiScope();

            var result = await _apiScopeCommandService.DeleteAsync(fakeApiScope.Id);

            result.IsSuccess.Should().BeTrue();

            var apiScope = await SingleOrDefaultAsync<ApiScope>(x => x.Id == fakeApiScope.Id);

            apiScope.Should().BeNull();
        }

        [Test]
        public async Task Should_return_failure_result_while_removing_api_scope_when_api_scope_is_not_exist()
        {

            var result = await _apiScopeCommandService.DeleteAsync(int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_add_new_api_scope_properties()
        {
            var fakeApiScope = await GenerateFakeApiScope();

            var model = new PropertyModel
            {
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _apiScopeCommandService.AddProperty(fakeApiScope.Id, model);

            result.IsSuccess.Should().BeTrue();

            var apiScope = await SingleAsync<ApiScope>(x => x.Id == fakeApiScope.Id);

            var property = apiScope.Properties.Single(x => x.Key == model.Key);

            property.Key.Should().Be(model.Key);

            property.Value.Should().Be(model.Value);

            apiScope.Properties.Count.Should().Be(fakeApiScope.Properties.Count + 1);

        }

        [Test]
        public async Task Should_return_failure_result_while_adding_property_when_api_scope_is_not_found()
        {
            var model = new PropertyModel
            {
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _apiScopeCommandService.AddProperty(int.MaxValue, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        [Test]
        public async Task Should_return_failure_result_while_adding_property_when_property_key_is_already_exist()
        {
            var fakeApiScope = await GenerateFakeApiScope();

            var fakeProperty = fakeApiScope.Properties.First();

            var model = new PropertyModel
            {
                Key = fakeProperty.Key,
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _apiScopeCommandService.AddProperty(fakeApiScope.Id, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<UserFriendlyException>();
        }

        [Test]
        public async Task Should_update_api_scope_property()
        {
            var fakeApiScope = await GenerateFakeApiScope();

            var fakeProperty = fakeApiScope.Properties.First();

            var model = new PropertyModel
            {
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _apiScopeCommandService.UpdateProperty(fakeApiScope.Id, fakeProperty.Id,model);

            result.IsSuccess.Should().BeTrue();

            var apiScope = await SingleAsync<ApiScope>(x => x.Id == fakeApiScope.Id);

            var property = apiScope.Properties.Single(x => x.Key == model.Key);

            property.Key.Should().Be(model.Key);

            property.Value.Should().Be(model.Value);

        }


        [Test]
        public async Task Should_return_failure_result_while_updating_property_when_api_scope_is_not_found()
        {
            var model = new PropertyModel
            {
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _apiScopeCommandService.UpdateProperty(int.MaxValue,int.MaxValue ,model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_property_when_api_scope_property_is_not_found()
        {
            var fakeApiScope = await GenerateFakeApiScope();

            var model = new PropertyModel
            {
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _apiScopeCommandService.UpdateProperty(fakeApiScope.Id, int.MaxValue, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_property_when_property_key_is_already_exist()
        {
            var fakeApiScope = await GenerateFakeApiScope();
            var firstFakeProperty = fakeApiScope.Properties.First();
            var lastFakeProperty = fakeApiScope.Properties.Last();

            var model = new PropertyModel
            {
                Key = lastFakeProperty.Key,
                Value = Guid.NewGuid().ToString(),
            };

            var result = await _apiScopeCommandService.UpdateProperty(fakeApiScope.Id,firstFakeProperty.Id, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<UserFriendlyException>();
        }

        [Test]
        public async Task Should_remove_api_scope_property()
        {
            var fakeApiScope = await GenerateFakeApiScope();

            var fakeProperty = fakeApiScope.Properties.First();

            var result = await _apiScopeCommandService.RemoveProperty(fakeApiScope.Id, fakeProperty.Id);

            result.IsSuccess.Should().BeTrue();


            var apiScope = await SingleAsync<ApiScope>(x => x.Id == fakeApiScope.Id);

            apiScope.Properties.Count.Should().Be(fakeApiScope.Properties.Count - 1);
        }

        [Test]
        public async Task Should_return_failure_result_while_removing_api_scope_property_when_api_scope_not_exist()
        {
            var result = await _apiScopeCommandService.RemoveProperty(int.MaxValue, int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_return_failure_result_while_removing_api_scope_property_when_api_scope_property_not_exist()
        {
            var fakeApiScope = await GenerateFakeApiScope();

            var result = await _apiScopeCommandService.RemoveProperty(fakeApiScope.Id, int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        protected Task<ApiScope> GenerateFakeApiScope()
        {
            var apiScope = new ApiScope
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                DisplayName = Guid.NewGuid().ToString(),

                Properties = new List<ApiScopeProperty>
                {
                    new ApiScopeProperty{Key = Guid.NewGuid().ToString() , Value= Guid.NewGuid().ToString()},
                    new ApiScopeProperty{Key = Guid.NewGuid().ToString() , Value= Guid.NewGuid().ToString()},
                    new ApiScopeProperty{Key = Guid.NewGuid().ToString() , Value= Guid.NewGuid().ToString()},
                    new ApiScopeProperty{Key = Guid.NewGuid().ToString() , Value= Guid.NewGuid().ToString()},
                }
            };

            return Insert(apiScope);
        }
        protected ApiScopeModel PrepareApiScopeModel()
        {
            return new ApiScopeModel
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                DisplayName = Guid.NewGuid().ToString(),
                ShowInDiscoveryDocument = true,
                Emphasize = true,
                UserClaims = new List<string>
                {
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString()
                },
                Properties = new List<PropertyModel>
                {
                    new PropertyModel{Key = Guid.NewGuid().ToString() , Value= Guid.NewGuid().ToString()},
                    new PropertyModel{Key = Guid.NewGuid().ToString() , Value= Guid.NewGuid().ToString()},
                    new PropertyModel{Key = Guid.NewGuid().ToString() , Value= Guid.NewGuid().ToString()},
                    new PropertyModel{Key = Guid.NewGuid().ToString() , Value= Guid.NewGuid().ToString()},
                },

            };
        }
    }
}
