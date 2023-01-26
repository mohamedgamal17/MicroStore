using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application.Users.Queries.GetUser;
using MicroStore.IdentityProvider.Identity.Infrastructure;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Queries.Tests.Users
{
    public class GetUserByIdQueryTests : BaseTestFixutre
    {
        [Test]
        public async Task Should_get_user_by_id()
        {
            var dbContext = ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();

            var fakeUser = await dbContext.Users.FirstAsync();

            var query = new GetUserByIdQuery
            {
                UserId = fakeUser.Id

            };

            var result = await Send(query);

            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var user = result.EnvelopeResult.Result;

            user.Id.Should().Be(query.UserId);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_while_user_id_is_not_exist()
        {
            var query = new GetUserByIdQuery
            {
                UserId = Guid.NewGuid()

            };

            var result = await Send(query);

            result.IsFailure.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        }


    }
}
