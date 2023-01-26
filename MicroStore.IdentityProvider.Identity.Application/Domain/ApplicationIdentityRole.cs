using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MicroStore.IdentityProvider.Identity.Application.Domain
{
    public class ApplicationIdentityRole : IdentityRole<Guid>
    {

        public string Description { get; set; }

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
                ClaimType= claim.Type,
                ClaimValue = claim.Value,
            });
        }


        public void  RemoveClaims(IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                RemoveClaim(claim);
            }
        }

        public void RemoveClaim(Claim claim)
        {
            var identityClaims = RoleClaims.Where(x=> x.ClaimType == claim.Type && x.ClaimValue == claim.Value).ToList();

            if(identityClaims.Any() )
            {
                foreach (var identityClaim in identityClaims)
                {
                    RoleClaims.Remove(identityClaim);
                }
            }
        }

    }

    public class ApplicationIdentityRoleClaim : IdentityRoleClaim<Guid>
    {

    }

    public class ApplicationIdentityUserRole : IdentityUserRole<Guid>
    {
        public ApplicationIdentityRole Role { get; set; }

        public ApplicationIdentityUserRole()
        {

        }
        public ApplicationIdentityUserRole(Guid userId , Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }

   
}
