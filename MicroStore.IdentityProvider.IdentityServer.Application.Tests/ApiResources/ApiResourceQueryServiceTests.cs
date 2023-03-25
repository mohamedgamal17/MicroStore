using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources;
using System.Net;
using Volo.Abp.Domain.Entities;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.ApiResources
{
    public class ApiResourceQueryServiceTests : BaseTestFixture
    {
        private readonly IApiResourceQueryService _apiResourceQueryService;

        public ApiResourceQueryServiceTests()
        {
            _apiResourceQueryService = GetRequiredService<IApiResourceQueryService>();  
        }

        [Test]
        public async Task Should_return_paged_list_api_resource()
        {
            var queryParams = new PagingQueryParams();

            var result = await _apiResourceQueryService.ListAsync(queryParams);

            result.IsSuccess.Should().BeTrue();

            result.Value.Lenght.Should().Be(queryParams.Lenght);

            result.Value.Items.Count().Should().BeLessThanOrEqualTo(queryParams.Lenght);
        }


        [Test]
        public async Task Should_return_api_resource()
        {
            var apiResource = await FirstAsync<ApiResource>();

            var result = await _apiResourceQueryService.GetAsync(apiResource.Id);

            result.IsSuccess.Should().BeTrue();

            result.Value.Id.Should().Be(apiResource.Id);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_api_resource_by_id_when_api_resource_is_not_exist()
        {

            var result = await _apiResourceQueryService.GetAsync(int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }
    }


}
