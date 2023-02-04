using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using MicroStore.IdentityProvider.IdentityServer.Application.Clients;
using System.Net;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.Clients
{
    public class When_receving_get_client_list_query : BaseTestFixture
    {
        [Test]
        public async Task Should_return_paged_list_client()
        {
            var query = new GetClientListQuery() { PageSize = 3 };

            var response = await Send(query);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.PageSize.Should().Be(query.PageSize);

            result.Items.Count().Should().BeLessThanOrEqualTo(result.PageSize);
        }
    }


    public class When_receving_get_client_query : BaseTestFixture
    {
        [Test]
        public async Task Should_return_client()
        {
            var client = await FirstAsync<Client>();

            var query = new GetClientQuery { ClientId = client.Id };

            var response = await Send(query);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Id.Should().Be(client.Id);

        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_while_client_is_not_exist()
        {
            var query = new GetClientQuery { ClientId = int.MaxValue };

            var response = await Send(query);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }


}
