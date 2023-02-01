using FluentAssertions;
using MicroStore.IdentityProvider.Identity.Application.Roles.Queries.GetRoleList;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Tests.Roles.Queries
{
    public class GetRoleListQueryTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_role_list()
        {
            var query = new GetRoleListQuery();

            var responseResult = await Send(query);

            responseResult.IsSuccess.Should().BeTrue();

            responseResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = responseResult.EnvelopeResult.Result;

            result.Items.Count.Should().BeGreaterThan(0);
        }
    }
}
