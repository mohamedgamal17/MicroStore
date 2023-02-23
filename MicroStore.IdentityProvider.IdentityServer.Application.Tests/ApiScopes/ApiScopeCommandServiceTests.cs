using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using MicroStore.IdentityProvider.IdentityServer.Application.Tests.Extensions;
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

            var apiScope = await SingleAsync<ApiScope>(x => x.Id == result.Result.Id);

            apiScope.AssertApiScope(result.Result);

            apiScope.AssertApiScopeCommand(model);
        }

        [Test]
        public async Task Should_update_api_scope()
        {
            var fakeApiScope = await GenerateFakeApiScope();

            var model = PrepareApiScopeModel();

            var result = await _apiScopeCommandService.UpdateAsync(fakeApiScope.Id,model);

            result.IsSuccess.Should().BeTrue();

            var apiScope = await SingleAsync<ApiScope>(x => x.Id == result.Result.Id);

            apiScope.AssertApiScope(result.Result);

            apiScope.AssertApiScopeCommand(model);
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_api_scope_when_api_scope_is_not_exist()
        {
            var model = PrepareApiScopeModel();

            var result = await _apiScopeCommandService.UpdateAsync(int.MaxValue,model);

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
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

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }


        protected Task<ApiScope> GenerateFakeApiScope()
        {
            var apiScope = new ApiScope
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                DisplayName = Guid.NewGuid().ToString(),
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
                    new PropertyModel{Key = Guid.NewGuid().ToString() , Value= Guid.NewGuid().ToString()}
                },

            };
        }
    }
}
