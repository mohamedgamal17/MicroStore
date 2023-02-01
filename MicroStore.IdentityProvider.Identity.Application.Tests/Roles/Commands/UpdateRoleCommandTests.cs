using FluentAssertions;
using MicroStore.IdentityProvider.Identity.Application.Roles.Commands.UpdateRole;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Tests.Roles.Commands
{
    public class UpdateRoleCommandTests : RoleCommandBaseTestFixture
    {

        [Test]
        public async Task Should_update_role()
        {
            var fakeRole = await CreateRole();
            var command = new UpdateRoleCommand
            {
                RoleId = fakeRole.Id,
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
            };

            var result = await Send(command);
            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var role = await FindRoleById(fakeRole.Id.ToString());

            role.Name.Should().Be(command.Name);
            role.Description.Should().Be(command.Description);
        }


        [Test]
        public async Task Should_reuturn_failure_result_with_status_code_404_notfound_when_role_is_not_exit()
        {
            var command = new UpdateRoleCommand
            {
                RoleId = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }


    }
}
