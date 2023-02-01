using FluentAssertions;
using MicroStore.IdentityProvider.Identity.Application.Roles.Commands.CreateRole;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Tests.Roles.Commands
{
    public class CreateRoleCommandTests : RoleCommandBaseTestFixture
    {
        [Test]
        public async Task Should_create_role()
        {
            var command = new CreateRoleCommand
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };

            var result = await Send(command);

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.Created);

            var role = await FindRoleById(result.EnvelopeResult.Result.Id.ToString());

            role.Name.Should().Be(command.Name);
            role.Description.Should().Be(command.Description);
        }

    }
}
