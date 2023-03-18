using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.Identity.Application.Users;
using MicroStore.IdentityProvider.Identity.Infrastructure.EntityFramework;
using Volo.Abp.Domain.Entities;

namespace MicroStore.IdentityProvider.Identity.Application.Tests.Users
{
    public class UserQueryServiceTests : BaseTestFixture
    {
        private readonly IUserQueryService _userQueryService;

        public UserQueryServiceTests()
        {
            _userQueryService= GetRequiredService<IUserQueryService>();
        }

        [Test]
        public async Task Should_get_user_paged_list()
        {
            var queryParams = new PagingQueryParams
            {
                PageNumber = 1,
                PageSize = 3
            };

            var result = await _userQueryService.ListAsync(queryParams);

            result.IsSuccess.Should().BeTrue();


            result.Value.PageNumber.Should().Be(queryParams.PageNumber);
            result.Value.PageSize.Should().Be(queryParams.PageSize);
            result.Value.Items.Count().Should().BeLessThanOrEqualTo(queryParams.PageSize);
        }


        [Test]
        public async Task Should_get_user_by_id()
        {
            var dbContext = ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();

            var fakeUser = await dbContext.Users.FirstAsync();

            var result = await _userQueryService.GetAsync(fakeUser.Id);

            result.IsSuccess.Should().BeTrue();

            result.Value.Id.Should().Be(fakeUser.Id);
        }

        [Test]
        public async Task Should_return_failure_while_getting_user_by_id_when_user_is_not_exist()
        {
            var result = await _userQueryService.GetAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }

        [Test]
        public async Task Should_get_user_by_email()
        {
            var dbContext = ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();

            var fakeUser = await dbContext.Users.FirstAsync();

            var result = await _userQueryService.GetByEmailAsync(fakeUser.Email);

            result.IsSuccess.Should().BeTrue();

            result.Value.Email.Should().Be(fakeUser.Email);
        }

        [Test]
        public async Task Should_return_failure_while_getting_user_by_email_when_user_is_not_exist()
        {
            var result = await _userQueryService.GetByEmailAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }



        [Test]
        public async Task Should_get_user_by_user_name()
        {
            var dbContext = ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();

            var fakeUser = await dbContext.Users.FirstAsync();

            var result = await _userQueryService.GetByUserNameAsync(fakeUser.UserName);

            result.IsSuccess.Should().BeTrue();

            result.Value.UserName.Should().Be(fakeUser.UserName);
        }

        [Test]
        public async Task Should_return_failure_while_getting_user_by_user_name_when_user_is_not_exist()
        {
            var result = await _userQueryService.GetByUserNameAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }
    }
}
