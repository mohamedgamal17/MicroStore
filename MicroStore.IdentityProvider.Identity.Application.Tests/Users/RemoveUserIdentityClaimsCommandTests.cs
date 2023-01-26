using FluentAssertions;
using MicroStore.IdentityProvider.Identity.Application.Users.Commands.AssignUserIdentityClaims;
using MicroStore.IdentityProvider.Identity.Application.Users.Commands.RemoveUserIdentityClaims;
using System.Net;
namespace MicroStore.IdentityProvider.Identity.Application.Tests.Users
{
    public class RemoveUserIdentityClaimsCommandTests : UserCommandBaseTestFixture
    {
        [Test]
        public async Task Should_remove_user_specific_claims()
        {
            var fakeClaims = CreateFakeClaims();
            var fakeUser = await CreateUserWithSpecificClaims(fakeClaims);

            var command = new RemoveUserIdentityClaimsCommand
            {
                UserId = fakeUser.Id,
                Claims = fakeClaims
            };

            var result = await Send(command);

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var user = await FindUserById(command.UserId);

            user.UserClaims.Count.Should().Be(0);
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
