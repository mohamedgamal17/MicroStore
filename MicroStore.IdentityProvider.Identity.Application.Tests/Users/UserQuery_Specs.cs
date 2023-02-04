using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application.Users;
using MicroStore.IdentityProvider.Identity.Infrastructure;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Tests.Users
{

    public class When_receiving_get_user_list_query : BaseTestFixture
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

    public class When_receiving_get_user_by_email_query  : BaseTestFixture
    {
        [Test]
        public async Task Should_get_user_by_email()
        {
            var dbContext = ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();

            var fakeUser = await dbContext.Users.FirstAsync();

            var query = new GetUserByEmailQuery
            {
                Email = fakeUser.Email

            };

            var result = await Send(query);

            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var user = result.EnvelopeResult.Result;

            user.Email.Should().Be(query.Email);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_while_user_email_is_not_exist()
        {
            var query = new GetUserByEmailQuery
            {
                Email = "NonExistEmail@example.com"

            };

            var result = await Send(query);

            result.IsFailure.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        }
    }

    public class When_receiving_get_user_by_id_query: BaseTestFixture
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
                UserId = Guid.NewGuid().ToString()

            };

            var result = await Send(query);

            result.IsFailure.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        }
    }
}
