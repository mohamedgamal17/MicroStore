using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes;
using Volo.Abp.Domain.Entities;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.ApiScopes
{
    public class ApiScopeQueryServiceTests : BaseTestFixture
    {

        private readonly IApiScopeQueryService _apiScopeQueryService;

        public ApiScopeQueryServiceTests()
        {
            _apiScopeQueryService = GetRequiredService<IApiScopeQueryService>();
        }

        [Test]
        public async Task Should_return_paged_list_api_scope()
        {
            var queryParams = new PagingQueryParams { PageSize = 3 };

            var result = await _apiScopeQueryService.ListAsync(queryParams);

            result.IsSuccess.Should().BeTrue();

            result.Value.PageSize.Should().Be(queryParams.PageSize);

            result.Value.Items.Count().Should().BeLessThanOrEqualTo(queryParams.PageSize);
        }

        [Test]
        public async Task Should_get_api_scope_by_id()
        {
            var apiScope = await FirstAsync<ApiScope>();

            var result = await _apiScopeQueryService.GetAsync(apiScope.Id);

            result.IsSuccess.Should().BeTrue();

            result.Value.Id.Should().Be(apiScope.Id);

        }

        [Test]
        public async Task Should_return_failure_result_while_getting_api_scope_by_id_when_apiscope_is_not_exist()
        {

            var result = await _apiScopeQueryService.GetAsync(int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }
    }

}
