using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Models;
using MicroStore.IdentityProvider.Identity.Application.Roles;
using MicroStore.IdentityProvider.Identity.Application.Tests.Extensions;
using Volo.Abp.Domain.Entities;

namespace MicroStore.IdentityProvider.Identity.Application.Tests.Roles
{ 
    public class RoleCommandServiceTests : BaseTestFixture
    {
        private readonly IRoleCommandService _roleCommandService;

        public RoleCommandServiceTests()
        {
            _roleCommandService= GetRequiredService<IRoleCommandService>();
        }
        [Test]
        public async Task Should_create_role()
        {
            var model = PreapreRoleModel();

            var result = await _roleCommandService.CreateAsync(model);

            result.IsSuccess.Should().BeTrue();

            var role = await FindRoleById(result.Value.Id);

            role.AssertRole(model);

            role.AssertRoleDto(result.Value);
        }


        [Test]
        public async Task Should_update_role()
        {
            var fakeRole = await CreateRole();

            var model = PreapreRoleModel();

            var result = await _roleCommandService.UpdateAsync(fakeRole.Id,model);

            result.IsSuccess.Should().BeTrue();

            var role = await FindRoleById(fakeRole.Id);

            role.AssertRole(model);

            role.AssertRoleDto(result.Value);
        }


        [Test]
        public async Task Should_reuturn_failure_result_while_updating_role_when_role_is_not_exit()
        {
            var model = PreapreRoleModel();

            var result = await _roleCommandService.UpdateAsync(Guid.NewGuid().ToString(), model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        protected RoleModel PreapreRoleModel()
        {
            return new RoleModel
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Claims = new List<IdentityClaimModel>
                {
                    new IdentityClaimModel
                    {
                        Type = Guid.NewGuid().ToString(),
                        Value = Guid.NewGuid().ToString()
                    }
                }
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

            var rolemanager = scope.ServiceProvider.GetRequiredService<ApplicationRoleManager>();

            var identityResult = await rolemanager.CreateAsync(identityRole);

            ThrowIfFailureResult(identityResult);

            return identityRole;
        }
    }

}
