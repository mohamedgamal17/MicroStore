using FluentAssertions;
using MicroStore.IdentityProvider.Identity.Application.Roles.Commands.AssignRoleIdentityClaims;
using System.Net;
namespace MicroStore.IdentityProvider.Identity.Application.Tests.Roles
{
    public class AssignRoleIdentityClaimsCommandTests : RoleCommandBaseTestFixture
    {

        [Test]
        public async Task Should_assign_role_identity_claims()
        {
            var fakeClaims = CreateFakeClaims();
            var fakeRole =await CreateRole();

            var command = new AssignRoleIdentityClaimsCommand
            {
                RoleId = fakeRole.Id,
                Claims = fakeClaims
            };

            var result = await Send(command);

            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var role = await FindRoleById(fakeRole.Id.ToString());
            
            role.RoleClaims.Count.Should().Be(fakeClaims.Count);

            role.RoleClaims.Should().Equal(fakeClaims, (left, right) => left.ClaimType == right.Type && left.ClaimValue == right.Value);
        }

        [Test]

        public async Task Should_reuturn_failure_result_with_status_code_404_notfound_when_role_is_not_exit()
        {
            var fakeClaims = CreateFakeClaims();

            var command = new AssignRoleIdentityClaimsCommand
            {
                RoleId = Guid.NewGuid(),
                Claims = fakeClaims
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

    }
}
