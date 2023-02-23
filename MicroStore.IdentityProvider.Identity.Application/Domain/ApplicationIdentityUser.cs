using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.Results;
using System.Security.Claims;

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

        public void AddRole(ApplicationIdentityRole role)
        {
            UserRoles.Add(new ApplicationIdentityUserRole(Id, role.Id));
        }

        public void AddUserClaims(IEnumerable<Claim> claims)
        {
            UserClaims.Clear();

            foreach (var claim in claims)
            {
                AddUserClaim(claim);
            }
        }
        public void AddUserClaim(Claim userClaim)
        {
            UserClaims.Add(new ApplicationIdentityUserClaim
            {
                UserId = Id,
                ClaimType = userClaim.Type,
                ClaimValue = userClaim.Value,
            });
        }

        public Result AddUserRoles(IEnumerable<ApplicationIdentityRole> identityRoles)
        {
            UserRoles.Clear();

            foreach (var role in identityRoles)
            {
                var result = AddUserRole(role);

                if (result.IsFailure)
                {
                    return result;
                }
            }

            return Result.Success();
        }

        public Result AddUserRole(ApplicationIdentityRole identityRole)
        {
            var role = UserRoles.SingleOrDefault(x => x.RoleId == identityRole.Id);

            if(role != null)
            {
                return Result.Failure("user_role_error", $"User is already assigned to role : {identityRole.Name}");
            }

            UserRoles.Add(new ApplicationIdentityUserRole
            {
                RoleId = identityRole.Id,
                UserId = Id,
            });

            return Result.Success();
        }

        public void AddUserLogin(ApplicationIdentityUserLogin userlogin)
        {
            UserLogins.Add(userlogin);
        }




        public void RemoveUserClaims(IEnumerable<Claim> userClaims)
        {
            foreach (var claim in userClaims)
            {
                RemoveUserClaim(claim);
            }
        }


        public void RemoveUserClaim(Claim userClaim)
        {
            var identityClaims = UserClaims.Where(x=> x.ClaimType == userClaim.Type && x.ClaimValue == userClaim.Value).ToList();

            if (identityClaims.Any())
            {
                foreach(var identityClaim  in identityClaims)
                {
                    UserClaims.Remove(identityClaim);
                }
            }
        }

        public Result RemoveUserRoles(IEnumerable<ApplicationIdentityRole> identityRoles)
        {
            foreach (var role in identityRoles)
            {
                var result = RemoveUserRole(role);

                if (result.IsFailure)
                {
                    return result;
                }
            }

            return Result.Success();
        }


        public Result RemoveUserRole(ApplicationIdentityRole identityRole)
        {
            var role = UserRoles.SingleOrDefault(x => x.RoleId == identityRole.Id);

            if (role == null)
            {
                return Result.Failure("user_role_error", $"User is not assigned to role : {identityRole.Name}");
            }

            UserRoles.Remove(role);

            return Result.Success();
        }
    }

    public class ApplicationIdentityUserClaim : IdentityUserClaim<string>
    {

    }
}
