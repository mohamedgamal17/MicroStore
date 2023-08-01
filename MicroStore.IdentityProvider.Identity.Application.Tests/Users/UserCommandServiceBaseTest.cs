using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;

namespace MicroStore.IdentityProvider.Identity.Application.Tests.Users
{
    public class UserCommandServiceBaseTest : BaseTestFixture
    {
        public async Task<ApplicationIdentityUser> CreateUser()
        {
            using var scope = ServiceProvider.CreateScope();

            using var usermanager = scope.ServiceProvider.GetRequiredService<ApplicationUserManager>();

            ApplicationIdentityUser user = new ApplicationIdentityUser
            {
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


        protected async Task<UserModel> PreapreUserModel()
        {
            var roles = await CreateFakeRoles();

            var command = new UserModel
            {
                GivenName = Guid.NewGuid().ToString(),
                FamilyName = Guid.NewGuid().ToString(),
                Email = $"{Guid.NewGuid().ToString()}@example.com",
                PhoneNumber = "447859305608",
                Password = Guid.NewGuid().ToString(),

                UserRoles = roles.Select(x => x.Name).ToList(),

            };

            return command;

        }
    }


}
