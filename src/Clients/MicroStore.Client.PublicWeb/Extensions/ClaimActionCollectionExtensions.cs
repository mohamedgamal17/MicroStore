using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System.Security.Claims;
using System.Text.Json;

namespace MicroStore.Client.PublicWeb.Extensions
{
    public static class ClaimActionCollectionExtensions
    {
        public static void MapInBoundCustomClaims(this ClaimActionCollection claimActions)
        {
            claimActions.MapJsonKey(ClaimTypes.Name, JwtClaimTypes.Name);
            claimActions.MapJsonKey(ClaimTypes.GivenName, JwtClaimTypes.GivenName);
            claimActions.MapJsonKey(ClaimTypes.Surname, JwtClaimTypes.FamilyName);
            claimActions.MapJsonKey(ClaimTypes.Email, JwtClaimTypes.Email);
            claimActions.MapJsonKey(ClaimTypes.MobilePhone, JwtClaimTypes.PhoneNumber);
            claimActions.Add(new RoleClaimAction());
        }
        private class RoleClaimAction : ClaimAction
        {
            public RoleClaimAction()
                : base(JwtClaimTypes.Role, ClaimValueTypes.String)
            {

            }

            public override void Run(JsonElement userData, ClaimsIdentity identity, string issuer)
            {
                var roles = userData.TryGetStringArray(JwtClaimTypes.Role)?.ToList();

                if (roles!.Any())
                {
                    foreach (var role in roles!)
                    {
                        AddRoleClaim(identity, role, issuer);
                    }

                    return;
                }
                else
                {
                    var singleRole = userData.TryGetString(JwtClaimTypes.Role);

                    if (!string.IsNullOrEmpty(singleRole))
                        AddRoleClaim(identity, singleRole, issuer);
                }
            }
            private void AddRoleClaim(ClaimsIdentity identity, string role, string issuer)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String, issuer));
            }


        }
    }
}
