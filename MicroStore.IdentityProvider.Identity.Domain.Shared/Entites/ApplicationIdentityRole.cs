using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MicroStore.IdentityProvider.Identity.Domain.Shared.Entites
{
    public class ApplicationIdentityRole : IdentityRole<string>
    {

        public string Description { get; set; }


        public ApplicationIdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }
        public List<ApplicationIdentityRoleClaim> RoleClaims { get; set; } = new List<ApplicationIdentityRoleClaim>();

        public void AddClaims(IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                AddClaim(claim);
            }
        }

        public void AddClaim(Claim claim)
        {
            RoleClaims.Add(new ApplicationIdentityRoleClaim
            {
                RoleId = Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
            });
        }


        public void RemoveClaims(IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                RemoveClaim(claim);
            }
        }

        public void RemoveClaim(Claim claim)
        {
            var identityClaims = RoleClaims.Where(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value).ToList();

            if (identityClaims.Any())
            {
                foreach (var identityClaim in identityClaims)
                {
                    RoleClaims.Remove(identityClaim);
                }
            }
        }

    }

    public class ApplicationIdentityRoleClaim : IdentityRoleClaim<string>
    {

    }

    public class ApplicationIdentityUserRole : IdentityUserRole<string>
    {
        public ApplicationIdentityRole Role { get; set; }

        public ApplicationIdentityUserRole()
        {

        }
        public ApplicationIdentityUserRole(string userId, string roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }


}
