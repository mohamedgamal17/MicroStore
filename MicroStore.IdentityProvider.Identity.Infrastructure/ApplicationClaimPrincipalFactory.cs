using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using System.Security.Claims;

namespace MicroStore.IdentityProvider.Identity.Infrastructure
{
    public class ApplicationClaimPrincipalFactory : UserClaimsPrincipalFactory<ApplicationIdentityUser, ApplicationIdentityRole>
    {
        public ApplicationClaimPrincipalFactory(UserManager<ApplicationIdentityUser> userManager, RoleManager<ApplicationIdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {

        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationIdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var identity = await GenerateClaimsAsync(user);

            if(user.GivenName != null)
            {
                identity.AddClaim(new Claim(JwtClaimTypes.GivenName, user.GivenName));
            }

            if(user.FamilyName != null)
            {
                identity.AddClaim(new Claim(JwtClaimTypes.FamilyName, user.FamilyName));
            }

            return new ClaimsPrincipal(identity);
        }


    }
}
