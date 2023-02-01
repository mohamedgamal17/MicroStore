using FluentAssertions;
using MicroStore.IdentityProvider.Identity.Application.Users.Queries.GetUserList;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Tests.Users.Queries
{
    public class GetUserListQueryTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_user_paged_list()
        {
            var query = new GetUserListQuery
            {
                PageNumber = 1,
                PageSize = 3
            };

            var responseResult = await Send(query);

            responseResult.IsSuccess.Should().BeTrue();
            responseResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = responseResult.EnvelopeResult.Result;

            result.PageNumber.Should().Be(query.PageNumber);
            result.PageSize.Should().Be(query.PageSize);
            result.Items.Count().Should().BeLessThanOrEqualTo(query.PageSize);
        }
    }
}
