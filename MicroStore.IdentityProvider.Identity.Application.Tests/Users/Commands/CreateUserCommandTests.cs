using FluentAssertions;
using MicroStore.IdentityProvider.Identity.Application.Users.Commands.CreateUser;
using System.Net;


namespace MicroStore.IdentityProvider.Identity.Application.Tests.Users.Commands
{
    public class CreateUserCommandTests : UserCommandBaseTestFixture
    {

        [Test]
        public async Task Should_create_user()
        {
            var command = PrepareCreateUserCommand();

            var result = await Send(command);

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.Created);

            var user = await FindUserById(result.EnvelopeResult.Result.Id);

            user.Email.Should().Be(command.Email);
            user.PhoneNumber.Should().Be(command.PhoneNumber);
            user.FirstName.Should().Be(command.FirstName);
            user.LastName.Should().Be(command.LastName);
            user.UserName.Should().Be(command.Email);

        }

        [Test]
        public async Task Should_return_failure_result_with_statuscode_400_bad_request_while_identity_result_failure()
        {
            var fakeUser = await CreateUser();

            var command = PrepareCreateUserCommand();

            command.Email = fakeUser.Email;

            var result = await Send(command);

            result.IsSuccess.Should().BeFalse();

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }



        public CreateUserCommand PrepareCreateUserCommand()
        {
            return new CreateUserCommand
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Email = $"{Guid.NewGuid().ToString()}@example.com",
                PhoneNumber = "447859305608",
                Password = Guid.NewGuid().ToString(),
            };
        }


    }
}
