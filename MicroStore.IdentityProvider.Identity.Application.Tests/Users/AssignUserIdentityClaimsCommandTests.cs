using FluentAssertions;
using MicroStore.IdentityProvider.Identity.Application.Users.Commands.AssignUserIdentityClaims;
using System.Net;
namespace MicroStore.IdentityProvider.Identity.Application.Tests.Users
{
    public class AssignUserIdentityClaimsCommandTests : UserCommandBaseTestFixture
    {
        [Test]
        public async Task Should_assign_user_to_specific_claims()
        {
            var fakeClaims = CreateFakeClaims();
            var fakeUser = await CreateUser();

            var command = new AssignUserIdentityClaimsCommand
            {
                UserId = fakeUser.Id,
                Claims = fakeClaims
            };

            var result = await Send(command);

            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var user = await FindUserById(command.UserId);

            user.UserClaims.Count.Should().Be(command.Claims.Count);

            user.UserClaims.Should().Equal(fakeClaims, (left, right) => left.ClaimType == right.Type && left.ClaimValue == right.Value);

        }


        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_when_user_is_not_exist()
        {
            var fakeClaims = CreateFakeClaims();

            var command = new AssignUserIdentityClaimsCommand
            {
                UserId = Guid.NewGuid(),
                Claims = fakeClaims
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }

}
