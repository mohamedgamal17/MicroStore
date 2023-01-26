using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Infrastructure;
using MicroStore.IdentityProvider.Identity.Infrastructure.Services;
using MicroStore.TestBase;
using System.Data;
using System.Security.Claims;

namespace MicroStore.IdentityProvider.Identity.Application.Tests
{
    [TestFixture]
    public abstract class BaseTestFixture : ApplicationTestBase<IdentityApplicationTestModule>
    {
        

        public async Task<ApplicationIdentityRole> UpdateRoleAsync(ApplicationIdentityRole identityRole)
        {
            using var scope = ServiceProvider.CreateScope();

            var rolemanager = scope.ServiceProvider.GetRequiredService<ApplicationRoleManager>();

            var identityResult = await rolemanager.UpdateAsync(identityRole);

            ThrowIfFailureResult(identityResult);

            return identityRole;
        }



        public async Task<ApplicationIdentityUser> FindUserById(Guid userId)
        {
            using var scope = ServiceProvider.CreateScope();

            var usermanager = scope.ServiceProvider.GetRequiredService<ApplicationUserManager>();

            return await usermanager.FindByIdAsync(userId.ToString());
        }

        public async Task<ApplicationIdentityRole> FindRoleById(string roleId)
        {
            using var scope = ServiceProvider.CreateScope();

            var rolemanager = scope.ServiceProvider.GetRequiredService<ApplicationRoleManager>();

            return await rolemanager.FindByIdAsync(roleId);
        }



        protected void ThrowIfFailureResult(IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                throw new InvalidOperationException(identityResult.Errors.JoinAsString("\n"));
            }
        }
    }
}