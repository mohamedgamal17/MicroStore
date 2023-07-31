using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace MicroStore.IdentityProvider.Host
{
    public class IdentityProviderConsts
    {
        public static List<string> AllowrdGrants { get; }
        static IdentityProviderConsts()
        {
            AllowrdGrants = new List<string>
            {
               OpenIdConnectGrantTypes.AuthorizationCode,
               OpenIdConnectGrantTypes.ClientCredentials ,
                OpenIdConnectGrantTypes.RefreshToken ,
                "urn:ietf:params:oauth:grant-type:token-exchange" 
            };
    }
}
}
