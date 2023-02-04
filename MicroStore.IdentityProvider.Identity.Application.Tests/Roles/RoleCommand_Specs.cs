using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application.Common.Models;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Roles;
using MicroStore.IdentityProvider.Identity.Application.Tests.Extensions;
using System.Net;
namespace MicroStore.IdentityProvider.Identity.Application.Tests.Roles
{ 
    public class When_reciveing_create_role_command : BaseTestFixture
    {
        [Test]
        public async Task Should_create_role()
        {
            var command = new CreateRoleCommand
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

            var result = await Send(command);

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.Created);

            var role = await FindRoleById(result.EnvelopeResult.Result.Id.ToString());

            role.AssertRole(command);

            role.AssertRoleDto(result.EnvelopeResult.Result);
        }

    }


    public class When_recivening_update_role_command : BaseTestFixture
    {
        [Test]
        public async Task Should_update_role()
        {
            var fakeRole = await CreateRole();
            var command = new UpdateRoleCommand
            {
                RoleId = fakeRole.Id,
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

            var result = await Send(command);
            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var role = await FindRoleById(fakeRole.Id.ToString());
            role.AssertRole(command);
            role.AssertRoleDto(result.EnvelopeResult.Result);
        }


        [Test]
        public async Task Should_reuturn_failure_result_with_status_code_404_notfound_when_role_is_not_exit()
        {
            var command = new UpdateRoleCommand
            {
                RoleId = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
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
