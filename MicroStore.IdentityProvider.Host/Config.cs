using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MicroStore.IdentityProvider.Identity.Application;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;

namespace MicroStore.IdentityProvider.Host
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "access_control",
                    DisplayName = "Access Control Resources",
                    ShowInDiscoveryDocument= true,
                    UserClaims = {JwtClaimTypes.Role },
                }

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
                new ApiScope("geographic.access", "access geogrpahic api"),
                new ApiScope("profiling.access","access profiling api"),

                new ApiScope("ordering.read","allowing read user orders operations"),
                new ApiScope("ordering.write","allowing write on user orders operations"),
                new ApiScope("billing.read","allowing read user payments operations"),
                new ApiScope("billing.write","allowing write on user payments operations"),
                new ApiScope("shipping.read","allowing read on user shipments operations"),

                
                new ApiScope("profiling.read","allowing read on user profile"),
                new ApiScope("profiling.write","allowing write on user profile"),
   



                new ApiScope("mvcgateway.ordering.read","allowing ordering read operation on micro store shopping api gateway"),
                new ApiScope("mvcgateway.ordering.write","allowing ordering write operation on micro store shopping api gateway"),
                new ApiScope("mvcgateway.billing.read","allowing billing read operation on micro store shopping api gateway"),
                new ApiScope("mvcgateway.billing.write","allowing billing write operation on micro store shopping api gateway"),
                new ApiScope("mvcgateway.shipping.read","allowing shipping read operation on micro store shopping api gateway"),
                new ApiScope("mvcgateway.inventory.read","allowing inventory read operation on micro store shopping api gateway"),
                new ApiScope("mvcgateway.inventory.write","allowing inventory write operation on micro store shopping api gateway"),

               new ApiScope("mvcgateway.profiling.read","allowing profiling read operation on micro store profiling api gateway"),
                new ApiScope("mvcgateway.profiling.write","allowing profiling write operation on micro store shopping api gateway"),
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
                    Scopes = {"inventory.access" }
                },

                new ApiResource("api-geographic","Geographic Api")
                {
                    Scopes = {"geographic.access"}
                },

                new ApiResource("api-profiling","Profiling Api")
                {
                    Scopes = { "profiling.access", "profiling.read", "profiling.write"}
                },

                new ApiResource("api-gateway","Shopping gateway")
                {
                    Scopes ={ "shoppinggateway.access" , "mvcgateway.ordering.read" , "mvcgateway.ordering.write" , "mvcgateway.billing.read" ,
                          "mvcgateway.billing.write" , "mvcgateway.shipping.read", "mvcgateway.inventory.read","mvcgateway.inventory.write" ,
                            "mvcgateway.profiling.access","mvcgateway.profiling.read",
                            "mvcgateway.profiling.write",
                          IdentityServerConstants.StandardScopes.OfflineAccess
                    },

                    ApiSecrets =  { new Secret("8832bcf5-6970-48f5-b9eb-86a62d104838".Sha512())},

                    UserClaims =
                    {
                        JwtClaimTypes.GivenName,
                        JwtClaimTypes.FamilyName,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.BirthDate,
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Role
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

                    AllowedScopes = { "catalog.access" , "basket.access" , "ordering.access" , "billing.access" , "shipping.access" , "inventory.access" ,"ordering.read" ,"ordering.write" , "billing.read" ,"billing.write" , "shipping.read", "api-sample" , "geographic.access",
                        "profiling.access",  "profiling.read", "profiling.write"

                    },


                    AllowOfflineAccess = false,
                    PairWiseSubjectSalt =  ""

                },

                // interactive client using code flow + pkce
                new Client
                {

                    ClientId = "microstoreinteractiveclient",
                    ClientSecrets = { new Secret("07366033-d7d3-46e9-9a4f-1f85ee7c9d17".Sha512()) },

                    AllowedGrantTypes = new List<string> { OpenIdConnectGrantTypes.AuthorizationCode , OpenIdConnectGrantTypes.RefreshToken,"urn:ietf:params:oauth:grant-type:token-exchange" },

                    AccessTokenType  = AccessTokenType.Reference,

                    AccessTokenLifetime = 120,

                    RefreshTokenExpiration = TokenExpiration.Sliding,

                    RefreshTokenUsage=  TokenUsage.OneTimeOnly,

                    AbsoluteRefreshTokenLifetime =(int) TimeSpan.FromDays(30).TotalSeconds,

                    SlidingRefreshTokenLifetime =(int) TimeSpan.FromDays(15).TotalSeconds,

                    ClientName ="Micro Store Interactive Client",

                    RedirectUris = { "https://localhost:7020/signin-oidc" },

                    FrontChannelLogoutUri = "https://localhost:7020/signout-oidc",

                    PostLogoutRedirectUris = { "https://localhost:7020/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "access_control", "shoppinggateway.access" ,"mvcgateway.ordering.read" , "mvcgateway.ordering.write", "mvcgateway.billing.read","mvcgateway.billing.write","mvcgateway.shipping.read","mvcgateway.inventory.read","mvcgateway.inventory.write" , "mvcgateway.profiling.read", "mvcgateway.profiling.write"},

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
                },
                new Client
                {
                    ClientId= "paymentapiswaggerclient",
                    ClientName= "Payment api tester client",
                    ClientSecrets = {new Secret ("6cd3c508-df1e-44d2-8dc6-a39925cbd263".Sha512()) } ,
                    AllowedGrantTypes =new List<string> { OpenIdConnectGrantTypes.AuthorizationCode ,  OpenIdConnectGrantTypes.ClientCredentials},
                    AllowedCorsOrigins = new List<string> { "https://localhost:7092"},
                    RedirectUris = { "https://localhost:7092/swagger/oauth2-redirect.html" },
                    AllowedScopes ={ "openid" , "profile" ,"billing.access" , "billing.read" , "billing.write" },
                    RequirePkce = true,
                    AccessTokenLifetime = 240
                },
                new Client
                {
                    ClientId = "shippingapiswaggerclient",
                    ClientName = "Shipping api tester client",
                    ClientSecrets = {new Secret("63742dbe-4e3a-4456-8b03-c3a93491a413".Sha512()) },
                    AllowedGrantTypes = new List<string> { OpenIdConnectGrantTypes.AuthorizationCode ,  OpenIdConnectGrantTypes.ClientCredentials},
                    AllowedCorsOrigins = { "https://localhost:7162" },
                    RedirectUris= { "https://localhost:7162/swagger/oauth2-redirect.html" },
                    AllowedScopes ={ "openid" , "profile" ,"shipping.access" , "shipping.read" },
                    RequirePkce=  true,
                    AccessTokenLifetime = 240

                },
                new Client
                {
                    ClientId ="orderingapiswaggerclient",
                    ClientName ="Ordering api tester client",
                    ClientSecrets = { new Secret("4216cce5-d583-4f66-887e-69801078f50c".Sha512()) },
                    AllowedGrantTypes =  new List<string>{ OpenIdConnectGrantTypes.AuthorizationCode ,  OpenIdConnectGrantTypes.ClientCredentials},
                    AllowedCorsOrigins = { "https://localhost:7226" },
                    RedirectUris = { "https://localhost:7226/swagger/oauth2-redirect.html" },
                    AllowedScopes = { "openid" , "profile"  , "ordering.access" , "ordering.read", "ordering.write" },
                    RequirePkce = true,
                    AccessTokenLifetime = 240
                },
                new Client
                {
                    ClientId="geographicapiinteractiveclient",
                    ClientName = "Geographic api tester client",
                    ClientSecrets = {new Secret ("c8b398e6-71c5-4004-99da-d872bf020851".Sha512()) },
                    AllowedGrantTypes = { OpenIdConnectGrantTypes.AuthorizationCode, OpenIdConnectGrantTypes.ClientCredentials },
                    AllowedCorsOrigins = { "https://localhost:7018" },
                    RedirectUris = { "https://localhost:7018/swagger/oauth2-redirect.html" },
                    AllowedScopes = { "openid", "profile"  , "geographic.access"},
                    RequirePkce= true,
                    AccessTokenLifetime = 240
                },
                new Client
                {
                    ClientId = "inventoryapiswaggerclient",
                    ClientName = "Inventory api tester client",
                    ClientSecrets = { new Secret("d2de8b36-a72f-4c0c-897d-3a6cc4669f7e".Sha512()) },
                    AllowedGrantTypes = {OpenIdConnectGrantTypes.AuthorizationCode, OpenIdConnectGrantTypes.ClientCredentials },
                    AllowedCorsOrigins = { "https://localhost:7054" },
                    RedirectUris = { "https://localhost:7054/swagger/oauth2-redirect.html" },
                    AllowedScopes = { "openid", "profile" , "inventory.access"}
                }
            };


        public static async Task SeedIdentityUsers(IServiceProvider serviceProvider)
        {
            var usermanager = serviceProvider.GetRequiredService<ApplicationUserManager>();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationIdentityRole>>();

            var adminRole = new ApplicationIdentityRole
            {
                Name = "Admin",
                Description = "Administration role"
            };

            var superAdminRole = new ApplicationIdentityRole
            {
                Name = "SuperAdmin",
                Description = "SuperAdmin Role for manage whole application"

            };


            await roleManager.CreateAsync(adminRole);

            await roleManager.CreateAsync(superAdminRole);

            var alice = new ApplicationIdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "AliceSmith@email.com",
                Email = "AliceSmith@email.com",
                GivenName = "alice",
                FamilyName = "smith",
                EmailConfirmed = true,
            };

            await usermanager.CreateAsync(alice, "pass123$");

            await usermanager.AddToRoleAsync(alice, adminRole.Name);

            await usermanager.AddToRoleAsync(alice, superAdminRole.Name);

            var bob = new ApplicationIdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                GivenName = "Bob",
                FamilyName = "Smith",
                UserName = "BobSmith@email.com",
                Email = "BobSmith@email.com",
                EmailConfirmed = true
            };

            await usermanager.CreateAsync(bob, "pass123$");



        }
    }
}