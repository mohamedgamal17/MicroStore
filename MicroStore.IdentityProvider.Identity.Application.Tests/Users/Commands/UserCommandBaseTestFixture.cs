using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application.Common.Models;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using System.Security.Claims;

namespace MicroStore.IdentityProvider.Identity.Application.Tests.Users.Commands
{
    public abstract class UserCommandBaseTestFixture : BaseTestFixture
    {
        public async Task<ApplicationIdentityUser> CreateUser()
        {
            using var scope = ServiceProvider.CreateScope();

            using var usermanager = scope.ServiceProvider.GetRequiredService<ApplicationUserManager>();

            ApplicationIdentityUser user = GenerateApplicationUser();

            await usermanager.CreateAsync(user);

            return user;
        }


        protected async Task<ApplicationIdentityUser> CreateUserWithSpecificRoles(List<string> roles)
        {
            using var scope = ServiceProvider.CreateScope();

            using var usermanager = scope.ServiceProvider.GetRequiredService<ApplicationUserManager>();

            var user = GenerateApplicationUser();

            await usermanager.CreateAsync(user, Guid.NewGuid().ToString());

            await usermanager.AddToRolesAsync(user, roles);

            return user;
        }


        private ApplicationIdentityUser GenerateApplicationUser()
        {
            return new ApplicationIdentityUser
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Email = $"{Guid.NewGuid().ToString()}@example.com",
                UserName = $"{Guid.NewGuid().ToString()}@example.com",
                PhoneNumber = "447859305608",
            };

        }

        protected async Task<ApplicationIdentityUser> CreateUserWithSpecificClaims(List<IdentityClaimModel> claims)
        {
            using var scope = ServiceProvider.CreateScope();

            using var usermanager = scope.ServiceProvider.GetRequiredService<ApplicationUserManager>();

            var user = GenerateApplicationUser();

            await usermanager.CreateAsync(user, Guid.NewGuid().ToString());

            await usermanager.AddClaimsAsync(user, claims.Select(x => new Claim(x.Type, x.Value)));

            return user;
        }

        protected async Task<List<ApplicationIdentityRole>> CreateFakeRoles()
        {
            var rolmanager = ServiceProvider.GetRequiredService<ApplicationRoleManager>();
            var rules = new List<ApplicationIdentityRole>
            {
                new ApplicationIdentityRole
                {
                    Id = Guid.NewGuid(),
                    Name = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                },
                new ApplicationIdentityRole
                {
                     Id = Guid.NewGuid(),
                    Name = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                },
                new ApplicationIdentityRole
                {
                     Id = Guid.NewGuid(),
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

}