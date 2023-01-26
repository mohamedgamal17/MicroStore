using FluentAssertions;
using MicroStore.IdentityProvider.Identity.Application.Roles.Commands.RemoveRoleIdentityClaims;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Tests.Roles
{
    public class RemoveRoleIdentityClaimsCommandTests : RoleCommandBaseTestFixture
    {
        [Test]
        public async Task Should_remove_role_identity_claims()
        {
            var fakeClaims = CreateFakeClaims();
            var fakeRole = await CreateRoleWithSpecificClaims(fakeClaims);

            var command = new RemoveRoleIdentityClaimsCommand
            {
                RoleId = fakeRole.Id,
                Claims = fakeClaims
            };

            var result = await Send(command);

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var role = await FindRoleById(result.EnvelopeResult.Result.Id.ToString());

            role.RoleClaims.Count().Should().Be(0);
        }


        [Test]

        public async Task Should_reuturn_failure_result_with_status_code_404_notfound_when_role_is_not_exit()
        {
            var fakeClaims = CreateFakeClaims();

            var command = new RemoveRoleIdentityClaimsCommand
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
