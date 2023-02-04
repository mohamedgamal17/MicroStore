using IdentityModel;
using MicroStore.IdentityProvider.Identity.Application;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Infrastructure;
using Serilog;
using System.Security.Claims;
using Volo.Abp.DependencyInjection;

namespace MicroStore.IdentityProvider.Host.Seeders
{
    [ExposeServices(typeof(IDataSeeder))]
    public class IdentityDataSeeder : IDataSeeder ,ITransientDependency
    {
        public async Task SeedAsync(DataSeederContext context, CancellationToken cancellationToken = default)
        {
            var dbContext = context.ServiceProvider.GetService<ApplicationIdentityDbContext>();

            var userMgr = context.ServiceProvider.GetRequiredService<ApplicationUserManager>();

            var alice = await userMgr.FindByNameAsync("alice");

            if (alice == null)
            {
                alice = new ApplicationIdentityUser
                {
                    FirstName = "alice",
                    LastName ="smith",
                    UserName = "alice",
                    Email = "AliceSmith@email.com",
                    EmailConfirmed = true,
                };
                var result = await userMgr.CreateAsync(alice, "pass123$");

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("alice created");
            }
            else
            {
                Log.Debug("alice already exists");
            }

            var bob = userMgr.FindByNameAsync("bob").Result;
            if (bob == null)
            {
                bob = new ApplicationIdentityUser
                {
                    FirstName = "bob",
                    LastName =  "smith",
                    UserName = "bob",
                    Email = "BobSmith@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(bob, "pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = await userMgr.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        });
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("bob created");
            }
            else
            {
                Log.Debug("bob already exists");
            }
        }
    }
}
