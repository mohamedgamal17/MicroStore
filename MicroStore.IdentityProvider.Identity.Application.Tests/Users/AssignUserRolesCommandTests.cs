using FluentAssertions;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Users.Commands.AssingUserRoles;
using System.Net;
namespace MicroStore.IdentityProvider.Identity.Application.Tests.Users
{
    public class AssignUserRolesCommandTests : UserCommandBaseTestFixture
    {

        [Test]
        public async Task Should_assign_user_role()
        {
            var fakeUser = await CreateUser();
            var fakeRules = await CreateFakeRoles();

            var command = new AssingUserRolesCommand
            {
                UserId = fakeUser.Id,
                Roles = fakeRules.Select(x => x.Name).ToList()
            };

            var result = await Send(command);

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.Created);

            var user = await FindUserById(fakeUser.Id);

            var userRoles =  user.UserRoles.Select(x => x.Role.Name).ToList();

            userRoles.Should().HaveCount(fakeRules.Count);

            userRoles.Should().BeEquivalentTo(command.Roles);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_when_user_is_not_exist()
        {
            var fakeRules = await CreateFakeRoles();

            var command = new AssingUserRolesCommand
            {
                UserId = Guid.NewGuid(),
                Roles = fakeRules.Select(x => x.Name).ToList()
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_400_bad_request_when_user_is_alread_assigned_to_role()
        {
            var fakeRoles = await CreateFakeRoles();

            var fakeUser = await CreateUserWithSpecificRoles(fakeRoles.Select(x => x.Name).ToList());

            var command = new AssingUserRolesCommand
            {
                UserId = fakeUser.Id,
                Roles = fakeRoles.Select(x => x.Name).ToList()
            };
            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        }

    }
}
