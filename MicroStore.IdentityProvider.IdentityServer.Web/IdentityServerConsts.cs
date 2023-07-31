using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace MicroStore.IdentityProvider.IdentityServer.Web
{
    public class IdentityServerConsts
    {

        public static Dictionary<string, string> AllowrdGrants{ get; }


        static IdentityServerConsts()
        {
            AllowrdGrants = new Dictionary<string, string>
            {
                { nameof(OpenIdConnectGrantTypes.AuthorizationCode) , OpenIdConnectGrantTypes.AuthorizationCode },
                { nameof(OpenIdConnectGrantTypes.ClientCredentials) , OpenIdConnectGrantTypes.ClientCredentials },
                { nameof(OpenIdConnectGrantTypes.RefreshToken) , OpenIdConnectGrantTypes.RefreshToken },
                { "TokenExchange","urn:ietf:params:oauth:grant-type:token-exchange" },
            };
        }
    }
}
