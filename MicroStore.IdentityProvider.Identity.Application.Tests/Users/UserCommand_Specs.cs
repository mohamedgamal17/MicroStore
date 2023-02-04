using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application.Common.Models;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Tests.Extensions;
using MicroStore.IdentityProvider.Identity.Application.Users;
using System.Net;

namespace MicroStore.IdentityProvider.Identity.Application.Tests.Users
{
    public class UserCommandBaseTest : BaseTestFixture
    {
        public async Task<ApplicationIdentityUser> CreateUser()
        {
            using var scope = ServiceProvider.CreateScope();

            using var usermanager = scope.ServiceProvider.GetRequiredService<ApplicationUserManager>();

            ApplicationIdentityUser user = new ApplicationIdentityUser
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Email = $"{Guid.NewGuid().ToString()}@example.com",
                UserName = $"{Guid.NewGuid().ToString()}@example.com",
                PhoneNumber = "447859305608",
            };

            await usermanager.CreateAsync(user);

            return user;
        }
        protected async Task<List<ApplicationIdentityRole>> CreateFakeRoles()
        {
            var rolmanager = ServiceProvider.GetRequiredService<ApplicationRoleManager>();
            var rules = new List<ApplicationIdentityRole>
            {
                new ApplicationIdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                },
                new ApplicationIdentityRole
                {
                     Id = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                },
                new ApplicationIdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                },

            };

            foreach (var item in rules)
            {
                var identityResult = await rolmanager.CreateAsync(item);
                ThrowIfFailureResult(identityResult);
            }

            return rules;
        }
        protected List<IdentityClaimModel> CreateFakeClaims()
        {
            return new List<IdentityClaimModel>
            {
                new IdentityClaimModel { Type = "Test", Value = Guid.NewGuid().ToString() },
                new IdentityClaimModel { Type = "Test", Value = Guid.NewGuid().ToString() },
                new IdentityClaimModel { Type = "Test", Value = Guid.NewGuid().ToString() }
            };
        }
    }

    public class When_reciveing_create_user_command : UserCommandBaseTest
    {
        [Test]
        public async Task Should_create_user()
        {
            var command =  await  PrepareCreateUserCommand();

            var result = await Send(command);

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.Created);

            var user = await FindUserById(result.EnvelopeResult.Result.Id);

            user.AssertUser(command);

            user.AssertUserPassword(command, ServiceProvider);

            user.AssertUserDto(result.EnvelopeResult.Result);
        }


        public async Task<CreateUserCommand> PrepareCreateUserCommand()
        {
            var roles = await CreateFakeRoles();

            var command = new CreateUserCommand
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Email = $"{Guid.NewGuid().ToString()}@example.com",
                PhoneNumber = "447859305608",
                Password = Guid.NewGuid().ToString(),

                UserRoles = roles.Select(x => x.Name).ToList(),

                UserClaims = CreateFakeClaims()
            };

            return command;

        }
    }


    public class When_reciveing_updat_user_command : UserCommandBaseTest
    {
        [Test]
        public async Task Should_update_user()
        {
            var fakeUser = await CreateUser();
            var command = await PrepareUpdateUserCommand();

            command.UserId = fakeUser.Id;

            var result = await Send(command);

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var user = await FindUserById(result.EnvelopeResult.Result.Id);

            user.AssertUser(command);

            user.AssertUserPassword(command, ServiceProvider);

            user.AssertUserDto(result.EnvelopeResult.Result);
        }


        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_when_user_is_not_exist()
        {

            var command = await PrepareUpdateUserCommand();

            command.UserId = Guid.NewGuid().ToString(); 

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        public async Task<UpdateUserCommand> PrepareUpdateUserCommand()
        {
            var roles = await CreateFakeRoles();

            var command = new UpdateUserCommand
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Email = $"{Guid.NewGuid().ToString()}@example.com",
                PhoneNumber = "447859305608",
                Password = Guid.NewGuid().ToString(),

                UserRoles = roles.Select(x => x.Name).ToList(),

                UserClaims = CreateFakeClaims()
            };

            return command;
        }

  
    }

}
