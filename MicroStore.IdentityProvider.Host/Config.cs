using Duende.IdentityServer.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MicroStore.IdentityProvider.Identity.Application;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using System.Security.Claims;

namespace MicroStore.IdentityProvider.Host
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha512()) },

                AllowedScopes = { "scope1" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "60beaa73-e505-439f-9c3c-6518e2e37a02",
                ClientSecrets = { new Secret("07366033-d7d3-46e9-9a4f-1f85ee7c9d17".Sha512()) },

                AllowedGrantTypes = new List<string> { OpenIdConnectGrantTypes.AuthorizationCode  , OpenIdConnectGrantTypes.ClientCredentials},

                RedirectUris = { "https://localhost:7020/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:7020/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:7020/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile"},

                RequirePkce = true,

                RequireConsent = true,


            },
            };


        public static async Task SeedIdentityUsers(IServiceProvider serviceProvider)
        {
            var usermanager = serviceProvider.GetRequiredService<ApplicationUserManager>();


            var alice = new ApplicationIdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "alice",
                LastName = "smith",
                UserName = "alice",
                Email = "AliceSmith@email.com",
                EmailConfirmed = true,
            };

            await usermanager.CreateAsync(alice, "pass123$");

            await usermanager.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                   });

            var bob = new ApplicationIdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "bob",
                LastName = "smith",
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = true
            };

            await usermanager.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        });
        }
    }
}