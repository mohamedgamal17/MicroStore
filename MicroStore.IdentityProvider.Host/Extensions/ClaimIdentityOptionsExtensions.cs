using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace MicroStore.IdentityProvider.Host.Extensions
{
    public static class ClaimIdentityOptionsExtensions
    {

        public static void MapClaimIdentity(this ClaimsIdentityOptions options)
        {
            options.UserIdClaimType = JwtClaimTypes.Subject;
            options.UserNameClaimType = JwtClaimTypes.Name;
            options.EmailClaimType = JwtClaimTypes.Email;
            options.RoleClaimType = JwtClaimTypes.Role;
        }

    }
}
