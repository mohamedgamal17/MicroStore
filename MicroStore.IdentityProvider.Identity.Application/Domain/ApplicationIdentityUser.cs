using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.Results;
using System.Security.Claims;
using Volo.Abp;

namespace MicroStore.IdentityProvider.Identity.Application.Domain
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ApplicationIdentityUser()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        public List<ApplicationIdentityUserClaim> UserClaims { get; set; } = new List<ApplicationIdentityUserClaim>();

        public List<ApplicationIdentityUserLogin> UserLogins { get; set; } = new List<ApplicationIdentityUserLogin>();

        public List<ApplicationIdentityUserToken> UserTokens { get; set; } = new List<ApplicationIdentityUserToken>();

        public List<ApplicationIdentityUserRole> UserRoles { get; set; } = new List<ApplicationIdentityUserRole>();

    }

    public class ApplicationIdentityUserClaim : IdentityUserClaim<string>
    {

    }
}
