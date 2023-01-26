using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application.Roles.Queries.GetRole;
using MicroStore.IdentityProvider.Identity.Infrastructure;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Queries.Tests.Roles
{
    public class GetRoleWithIdQueryTests : BaseTestFixutre
    {
        [Test]
        public async Task Should_get_role_by_id()
        {
            var dbContext = ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();

            var fakeRole = await dbContext.Roles.FirstAsync();

            var query = new GetRoleWithIdQuery
            {
                Id = fakeRole.Id
            };

            var responseResult = await Send(query);

            responseResult.IsSuccess.Should().BeTrue();

            responseResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var role = responseResult.EnvelopeResult.Result;

            role.Id.Should().Be(query.Id);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_when_role_id_is_not_exist()
        {

            var query = new GetRoleWithIdQuery
            {
                Id = Guid.NewGuid()
            };

            var responseResult = await Send(query);

            responseResult.IsFailure.Should().BeTrue();

            responseResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
