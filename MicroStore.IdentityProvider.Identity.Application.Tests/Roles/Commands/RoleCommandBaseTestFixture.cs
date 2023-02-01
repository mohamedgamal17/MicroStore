using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application.Common.Models;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using System.Security.Claims;
namespace MicroStore.IdentityProvider.Identity.Application.Tests.Roles.Commands
{
    public abstract class RoleCommandBaseTestFixture : BaseTestFixture
    {

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

        protected async Task<ApplicationIdentityRole> CreateRoleWithSpecificClaims(List<IdentityClaimModel> claims)
        {

            using var scope = ServiceProvider.CreateScope();

            var rolemanager = scope.ServiceProvider.GetRequiredService<ApplicationRoleManager>();

            var identityRole = new ApplicationIdentityRole
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
            };

            identityRole.AddClaims(claims.Select(x => new Claim(x.Type, x.Value)));


            var identityResult = await rolemanager.CreateAsync(identityRole);

            ThrowIfFailureResult(identityResult);


            return identityRole;

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
}
