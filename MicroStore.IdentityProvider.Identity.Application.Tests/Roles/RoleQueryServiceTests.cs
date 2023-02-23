using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.Identity.Application.Roles;
using MicroStore.IdentityProvider.Identity.Infrastructure.EntityFramework;
using System.Net;
namespace MicroStore.IdentityProvider.Identity.Application.Tests.Roles
{
    public class RoleQueryServiceTests : BaseTestFixture
    {
        private readonly IRoleQueryService _roleQueryService;

        public RoleQueryServiceTests()
        {
            _roleQueryService= GetRequiredService<IRoleQueryService>();
        }

        [Test]
        public async Task Should_get_role_list()
        {
            var result = await _roleQueryService.ListAsync();

            result.IsSuccess.Should().BeTrue();

            result.Value.Count.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task Should_get_role_by_name()
        {
            var dbContext = ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();

            var fakeRole = await dbContext.Roles.FirstAsync();

            var result = await _roleQueryService.GetByNameAsync(fakeRole.Name);

            result.IsSuccess.Should().BeTrue();

            result.Value.Name.Should().Be(fakeRole.Name);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_role_by_name_when_role_is_not_exist()
        {
            var result = await _roleQueryService.GetByNameAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }


        [Test]
        public async Task Should_get_role_by_id()
        {
            var dbContext = ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();

            var fakeRole = await dbContext.Roles.FirstAsync();

            var result = await _roleQueryService.GetAsync(fakeRole.Id);

            result.IsSuccess.Should().BeTrue();

            result.Value.Id.Should().Be(fakeRole.Id);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_role_by_id_when_role_is_not_exist()
        {
            var result = await _roleQueryService.GetAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }

    }
}
