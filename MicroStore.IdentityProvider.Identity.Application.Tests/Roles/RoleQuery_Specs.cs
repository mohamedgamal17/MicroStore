using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application.Roles;
using MicroStore.IdentityProvider.Identity.Infrastructure;
using System.Net;
namespace MicroStore.IdentityProvider.Identity.Application.Tests.Roles
{
    public class When_reciveing_get_role_list_query : BaseTestFixture
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

    public class When_reciveing_get_role_with_name_query : BaseTestFixture
    {
        [Test]
        public async Task Should_get_role_by_name()
        {
            var dbContext = ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();

            var fakeRole = await dbContext.Roles.FirstAsync();

            var query = new GetRoleWithNameQuery
            {
                Name = fakeRole.Name,
            };

            var responseResult = await Send(query);

            responseResult.IsSuccess.Should().BeTrue();

            responseResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var role = responseResult.EnvelopeResult.Result;

            role.Name.Should().Be(query.Name);
        }


        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_when_role_name_is_not_exist()
        {

            var query = new GetRoleWithNameQuery
            {
                Name = Guid.NewGuid().ToString()
            };

            var responseResult = await Send(query);

            responseResult.IsFailure.Should().BeTrue();

            responseResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }


    public class When_reciveing_get_role_with_id_query : BaseTestFixture
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
                Id = Guid.NewGuid().ToString()
            };

            var responseResult = await Send(query);

            responseResult.IsFailure.Should().BeTrue();

            responseResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
