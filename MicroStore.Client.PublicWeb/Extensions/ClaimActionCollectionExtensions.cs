using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System.Security.Claims;

namespace MicroStore.Client.PublicWeb.Extensions
{
    public static class ClaimActionCollectionExtensions
    {
        public static void MapInBoundCustomClaims(this ClaimActionCollection claimActions)
        {
            claimActions.MapUniqueJsonKey(ClaimTypes.GivenName, JwtClaimTypes.GivenName);
            claimActions.MapUniqueJsonKey(ClaimTypes.Surname, JwtClaimTypes.FamilyName);
            claimActions.MapUniqueJsonKey(ClaimTypes.Email, JwtClaimTypes.Email);
            claimActions.MapUniqueJsonKey(ClaimTypes.MobilePhone, JwtClaimTypes.PhoneNumber);
            claimActions.MapUniqueJsonKey(ClaimTypes.Role, JwtClaimTypes.Role);

        }
    }
}
