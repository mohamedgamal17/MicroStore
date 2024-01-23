using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace MicroStore.Bff.Shopping.Helpers
{
    public class ScopeClaimsTransformer : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identities = new List<ClaimsIdentity>();

            foreach (var id in principal.Identities)
            {
                var identity = new ClaimsIdentity(id.AuthenticationType, id.NameClaimType, id.RoleClaimType);

                foreach (var claim in id.Claims)
                {
                    if (claim.Type == "scope")
                    {
                        if (claim.Value.Contains(' '))
                        {
                            var scopes = claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                            foreach (var scope in scopes)
                            {
                                identity.AddClaim(new Claim("scope", scope, claim.ValueType, claim.Issuer));
                            }
                        }
                        else
                        {
                            identity.AddClaim(claim);
                        }
                    }
                    else
                    {
                        identity.AddClaim(claim);
                    }
                }

                identities.Add(identity);
            }

            return Task.FromResult(new ClaimsPrincipal(identities));
        }
    }
}
