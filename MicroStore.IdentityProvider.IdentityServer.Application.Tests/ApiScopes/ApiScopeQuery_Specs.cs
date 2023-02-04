using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes;
using System.Net;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.ApiScopes
{
    public class When_receving_get_api_scope_list_query : BaseTestFixture
    {
        [Test]
        public async Task Should_return_paged_list_api_scope()
        {
            var query = new GetApiScopeListQuery { PageSize = 3 };

            var response = await Send(query);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.PageSize.Should().Be(3);

            result.Items.Count().Should().BeLessThanOrEqualTo(query.PageSize);
        }


    }


    public class When_receving_get_api_scope_query : BaseTestFixture
    {
        [Test]
        public async Task Should_return_api_scope()
        {
            var apiScope = await FirstAsync<ApiScope>();

            var query = new GetApiScopeQuery { ApiScopeId = apiScope.Id };

            var response = await Send(query);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Id.Should().Be(apiScope.Id);
        }


        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_while_apiscope_is_not_exist()
        {
            var query = new GetApiScopeQuery { ApiScopeId = int.MaxValue };

            var response = await Send(query);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
