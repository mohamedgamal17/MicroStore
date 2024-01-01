using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application.Roles;
using MicroStore.IdentityProvider.Identity.Application.Tests.Extensions;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;
using Volo.Abp.Domain.Entities;

namespace MicroStore.IdentityProvider.Identity.Application.Tests.Roles
{
    public class RoleCommandServiceTests : BaseTestFixture
    {
        private readonly IRoleCommandService _roleCommandService;

        public RoleCommandServiceTests()
        {
            _roleCommandService = GetRequiredService<IRoleCommandService>();
        }
        [Test]
        public async Task Should_create_role()
        {
            var model = PrepareRoleModel();

            var result = await _roleCommandService.CreateAsync(model);

            result.IsSuccess.Should().BeTrue();

            var role = await SingleAsync<ApplicationIdentityRole>(x => x.Id == result.Value.Id);

            role.AssertRole(model);

            role.AssertRoleDto(result.Value);
        }



        [Test]
        public async Task Should_update_role()
        {
            var fakeRole = await CreateRole();

            var model = PrepareRoleModel();

            var result = await _roleCommandService.UpdateAsync(fakeRole.Id, model);

            result.IsSuccess.Should().BeTrue();

            var role = await SingleOrDefaultAsync<ApplicationIdentityRole>(x => x.Id == fakeRole.Id);

            role.AssertRole(model);

            role.AssertRoleDto(result.Value);
        }


        [Test]
        public async Task Should_reuturn_failure_result_while_updating_role_when_role_is_not_exit()
        {
            var model = PrepareRoleModel();

            var result = await _roleCommandService.UpdateAsync(Guid.NewGuid().ToString(), model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        [Test]
        public async Task Should_remove_role()
        {
            var fakeRole = await CreateRole();

            var model = PrepareRoleModel();

            var result = await _roleCommandService.RemoveAsync(fakeRole.Id);

            result.IsSuccess.Should().BeTrue();

            var role = await SingleOrDefaultAsync<ApplicationIdentityRole>(x => x.Id == fakeRole.Id);

            role.Should().BeNull();
        }


        [Test]
        public async Task Should_return_failure_result_while_removing_rule_when_rule_is_not_exist()
        {
            var roleId = Guid.NewGuid().ToString();

            var result = await _roleCommandService.RemoveAsync(roleId);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        protected RoleModel PrepareRoleModel()
        {
            return new RoleModel
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),

            };
        }

        protected async Task<ApplicationIdentityRole> CreateRole()
        {
            using var scope = ServiceProvider.CreateScope();

            var identityRole = new ApplicationIdentityRole
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
            };

            var rolemanager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationIdentityRole>>();

            var identityResult = await rolemanager.CreateAsync(identityRole);

            ThrowIfFailureResult(identityResult);

            return identityRole;
        }
    }

}
