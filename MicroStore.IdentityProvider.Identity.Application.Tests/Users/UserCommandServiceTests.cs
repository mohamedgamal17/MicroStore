using FluentAssertions;
using MicroStore.IdentityProvider.Identity.Application.Tests.Extensions;
using MicroStore.IdentityProvider.Identity.Application.Users;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using Volo.Abp.Domain.Entities;

namespace MicroStore.IdentityProvider.Identity.Application.Tests.Users
{

    public class UserCommandServiceTests : UserCommandServiceBaseTest
    {
        private readonly IUserCommandService _userCommandService;

        public UserCommandServiceTests()
        {
            _userCommandService = GetRequiredService<IUserCommandService>();
        }

        [Test]
        public async Task Should_create_user()
        {
            var model = await PreapreUserModel();

            var result = await _userCommandService.CreateUserAsync(model);

            result.IsSuccess.Should().BeTrue();

            var user = await SingleAsync<ApplicationIdentityUser>(x=> x.Id == result.Value.Id);

            user.AssertUser(model);

            user.AssertUserPassword(model, ServiceProvider);

            user.AssertUserDto(result.Value);
        }

        [Test]
        public async Task Should_update_user()
        {
            var fakeUser = await CreateUser();

            var model = await PreapreUserModel();

            var result = await _userCommandService.UpdateUserAsync(fakeUser.Id,model);

            result.IsSuccess.Should().BeTrue();

            var user = await  SingleAsync<ApplicationIdentityUser>(x => x.Id == result.Value.Id);

            user.AssertUser(model);

            user.AssertUserPassword(model, ServiceProvider);

            user.AssertUserDto(result.Value);
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_user_when_user_is_not_exist()
        {
            var model = await PreapreUserModel();

            var result = await _userCommandService.UpdateUserAsync(Guid.NewGuid().ToString(), model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }



    }


}
