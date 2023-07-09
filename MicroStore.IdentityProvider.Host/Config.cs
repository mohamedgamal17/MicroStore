using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;
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
                new ApiScope("shoppinggateway.access","Shopping api gateway"),

                new ApiScope("catalog.access","access catalog api"),
                new ApiScope("basket.access","access basket api"),
                new ApiScope("ordering.access","acccess ordering api"),
                new ApiScope("shipping.access","access shipping api"),
                new ApiScope("billing.access","access billing api"),
                new ApiScope("inventory.access","access inventory api"),


                new ApiScope("ordering.read","allowing read user orders operations"),
                new ApiScope("ordering.write","allowing write on user orders operations"),
                new ApiScope("billing.read","allowing read user payments operations"),
                new ApiScope("billing.write","allowing write on user payments operations"),
                new ApiScope("shipping.read","allowing read on user shipments operations"),
                new ApiScope("inventory.write","allowing write on user orders operations"),
                new ApiScope("inventory.read","allowing read on user orders operations"),


                new ApiScope("mvcgateway.ordering.read","allowing ordering read operation on micro store shopping api gateway"),
                new ApiScope("mvcgateway.ordering.write","allowing ordering write operation on micro store shopping api gateway"),
                new ApiScope("mvcgateway.billing.read","allowing billing read operation on micro store shopping api gateway"),
                new ApiScope("mvcgateway.billing.write","allowing billing write operation on micro store shopping api gateway"),
                new ApiScope("mvcgateway.shipping.read","allowing shipping read operation on micro store shopping api gateway"),
                new ApiScope("mvcgateway.inventory.read","allowing inventory read operation on micro store shopping api gateway"),
                new ApiScope("mvcgateway.inventory.write","allowing inventory write operation on micro store shopping api gateway"),
            };




        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("api-sample" , "Api Sample App" )
                {
                    Description = "Sample APi",
                    Scopes = { "sample.access" }
                },

                new ApiResource("api-catalog","Catalog Api")
                {
                    Scopes = { "catalog.access" }
                },

                new ApiResource("api-basket","Basket Api")
                {
                    Scopes = {"basket.access"}
                },

                new ApiResource("api-ordering","Ordering Api")
                {
                    Scopes = {"ordering.access" , "ordering.read", "ordering.write"},

                },

                new ApiResource("api-billing","Billing Api")
                {
                    Scopes = {"billing.access" , "billing.read" , "billing.write" }
                },

                new ApiResource("api-shipping","Shipping Api")
                {
                    Scopes = {"shipping.access","shipping.read"}
                },

                new ApiResource("api-inventory" ,"Inventory APi")
                {
                    Scopes = {"inventory.access","inventory.read", "inventory.write" }
                },

                new ApiResource("api-gateway","Shopping gateway")
                {
                    Scopes ={ "shoppinggateway.access" , "mvcgateway.ordering.read" , "mvcgateway.ordering.write" , "mvcgateway.billing.read" ,
                          "mvcgateway.billing.write" , "mvcgateway.shipping.read", "mvcgateway.inventory.read","mvcgateway.inventory.write" ,
                          IdentityServerConstants.StandardScopes.OfflineAccess
                    }
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "gatewaytodownstreamtokenexchangeclient",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = new List<string> { "urn:ietf:params:oauth:grant-type:token-exchange"},
                    ClientSecrets = { new Secret("07366033-d7d3-46e9-9a4f-1f85ee7c9d17".Sha512()) },

                    AllowedScopes = { "catalog.access" , "basket.access" , "ordering.access" , "billing.access" , "shipping.access" , "inventory.access" ,"ordering.read" ,"ordering.write" , "billing.read" ,"billing.write" , "shipping.read","inventory.write" , "inventory.read" , "api-sample"},


                    AllowOfflineAccess = false,

                },

                // interactive client using code flow + pkce
                new Client
                {

                    ClientId = "microstoreinteractiveclient",
                    ClientSecrets = { new Secret("07366033-d7d3-46e9-9a4f-1f85ee7c9d17".Sha512()) },

                    AllowedGrantTypes = new List<string> { OpenIdConnectGrantTypes.AuthorizationCode , OpenIdConnectGrantTypes.RefreshToken,"urn:ietf:params:oauth:grant-type:token-exchange" },

                    AccessTokenLifetime = 120,

                    RefreshTokenExpiration = TokenExpiration.Absolute,

                    RefreshTokenUsage=  TokenUsage.OneTimeOnly,

                    ClientName ="Micro Store Interactive Client",

                    RedirectUris = { "https://localhost:7020/signin-oidc" },

                    FrontChannelLogoutUri = "https://localhost:7020/signout-oidc",

                    PostLogoutRedirectUris = { "https://localhost:7020/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "shoppinggateway.access" ,"mvcgateway.ordering.read" , "mvcgateway.ordering.write", "mvcgateway.billing.read","mvcgateway.billing.write","mvcgateway.shipping.read","mvcgateway.inventory.read","mvcgateway.inventory.write" },

                    RequirePkce = true,

                    RequireConsent = true,



                },
                 new Client
                {
                    ClientId = "catalogapiinteractiveclient",
                    ClientName ="Catalog api tester client",
                    ClientSecrets = { new Secret("cf0bc7fb-3796-4b45-9d93-364d7c28083b".Sha512()) },
                    AllowedGrantTypes = new List<string> { OpenIdConnectGrantTypes.AuthorizationCode ,  OpenIdConnectGrantTypes.ClientCredentials},
                    AllowedCorsOrigins = new List<string> { "https://localhost:7188"},
                    RedirectUris = { "https://localhost:7188/swagger/oauth2-redirect.html" },
                    AllowedScopes ={ "openid" , "profile" ,"catalog.access" },
                    RequirePkce = true,
                    AccessTokenLifetime = 240


                },
                new Client
                {
                    ClientId = "basketapiintercativeclient",
                    ClientName= "Basket api tester client",
                    ClientSecrets = {new Secret ("3e3e0d26-75e5-463f-80f0-cd0b3a3a7f99".Sha512()) } ,
                    AllowedGrantTypes =new List<string> { OpenIdConnectGrantTypes.AuthorizationCode ,  OpenIdConnectGrantTypes.ClientCredentials},
                    AllowedCorsOrigins = new List<string> { "https://localhost:7268/"},
                    RedirectUris = { "https://localhost:7268/swagger/oauth2-redirect.html" },
                    AllowedScopes ={ "openid" , "profile" ,"basket.access" },
                    RequirePkce = true,
                    AccessTokenLifetime = 240
                }
            };


        public static async Task SeedIdentityUsers(IServiceProvider serviceProvider)
        {
            var usermanager = serviceProvider.GetRequiredService<ApplicationUserManager>();


            var alice = new ApplicationIdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "alice",
                Email = "AliceSmith@email.com",
                EmailConfirmed = true,
            };

            await usermanager.CreateAsync(alice, "pass123$");

            await usermanager.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "alice"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                   });

            var bob = new ApplicationIdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = true
            };

            await usermanager.CreateAsync(bob, "pass123$");

            await usermanager.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "bob"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        });
        }
    }
}