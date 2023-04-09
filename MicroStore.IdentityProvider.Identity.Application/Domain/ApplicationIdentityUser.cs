using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MicroStore.IdentityProvider.Identity.Application.Domain
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }

        public List<ApplicationIdentityUserClaim> UserClaims { get; set; }

        public List<ApplicationIdentityUserLogin> UserLogins { get; set; }

        public List<ApplicationIdentityUserToken> UserTokens { get; set; }

        public List<ApplicationIdentityUserRole> UserRoles { get; set; }

        public ApplicationIdentityUser()
        {
            Id = Guid.NewGuid().ToString();
            UserClaims = new List<ApplicationIdentityUserClaim>();
            UserLogins = new List<ApplicationIdentityUserLogin>();
            UserTokens = new List<ApplicationIdentityUserToken>();
            UserRoles = new List<ApplicationIdentityUserRole>();
        }



        public void ReplaceClaim(Claim claim)
        {
            var replacedClaim = UserClaims.SingleOrDefault(x => x.ClaimType == claim.Type);

            if(replacedClaim != null)
            {
                UserClaims.Remove(replacedClaim);
            }

            UserClaims.Add(new ApplicationIdentityUserClaim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            });
        }


        public void ReplaceClaim(string type, string value)
        {
            ReplaceClaim(new Claim(type,value) );
        }


    }

    public class ApplicationIdentityUserClaim : IdentityUserClaim<string>
    {

    }
}
