using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources;
using System.Net;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.ApiResources
{
    public class When_receving_get_api_resource_list_query : BaseTestFixture
    {
        [Test]
        public async Task Should_return_paged_list_api_resource()
        {
            var query = new GetApiResourceListQuery { PageSize = 3 };

            var response = await Send(query);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.PageSize.Should().Be(query.PageSize);

            result.Items.Count().Should().BeLessThanOrEqualTo(query.PageSize);
        }
    }

    public class When_receving_get_api_resource_query : BaseTestFixture
    {
        [Test]
        public async Task Should_return_api_resource()
        {
            var apiResource = await FirstAsync<ApiResource>();

            var query = new GetApiResourceQuery { ApiResourceId = apiResource.Id };

            var response = await Send(query);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Id.Should().Be(apiResource.Id);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_while_api_resource_is_not_exist()
        {
            var query = new GetApiResourceQuery { ApiResourceId = int.MaxValue };

            var response = await Send(query);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        }
    }

}
