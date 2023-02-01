using FluentAssertions;
using MicroStore.IdentityProvider.Identity.Application.Users.Commands.RemoveUserRoles;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Tests.Users.Commands
{
    public class RemoveUserRolesCommandTests : UserCommandBaseTestFixture
    {
        [Test]
        public async Task Should_remove_user_from_specific_roles()
        {
            var fakeRoles = await CreateFakeRoles();
            var fakeUser = await CreateUserWithSpecificRoles(fakeRoles.Select(x => x.Name).ToList());

            var command = new RemoveUserRolesCommand
            {
                UserId = fakeUser.Id,
                Roles = fakeRoles.Select(x => x.Name).ToList()
            };

            var result = await Send(command);

            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var user = await FindUserById(command.UserId);
            user.UserRoles.Count.Should().Be(0);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_when_user_is_not_exist()
        {
            var fakeRules = await CreateFakeRoles();

            var command = new RemoveUserRolesCommand
            {
                UserId = Guid.NewGuid(),
                Roles = fakeRules.Select(x => x.Name).ToList()
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }


        [Test]
        public async Task Should_return_failure_result_with_status_code_400_bad_request_when_user_is_assigned_to_role()
        {
            var fakeRules = await CreateFakeRoles();
            var fakeUser = await CreateUser();

            var command = new RemoveUserRolesCommand
            {
                UserId = fakeUser.Id,
                Roles = fakeRules.Select(x => x.Name).ToList()
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }


    }
}
