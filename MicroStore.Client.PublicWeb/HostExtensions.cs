using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using MicroStore.Client.PublicWeb.Infrastructure;
using System.IdentityModel.Tokens.Jwt;

namespace MicroStore.Client.PublicWeb
{
    public static class HostExtensions
    {
        public static WebApplication ConfigureService(this WebApplicationBuilder builder)
        {
            var config = builder.Configuration;

            builder.Services.AddRazorPages()
             .AddRazorPagesOptions(opt =>
            {
                opt.Conventions.AddPageRoute("/FrontEnd/Home", "");

            }).AddRazorRuntimeCompilation();

            builder.Services.AddControllersWithViews();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = config.GetValue<string>("IdentityProvider:Authority");
                options.ClientId = config.GetValue<string>("IdentityProvider:ClientId");
                options.ClientSecret = config.GetValue<string>("IdentityProvider:ClientSecret");
                options.GetClaimsFromUserInfoEndpoint = true;
                options.UsePkce = true;
                options.ResponseType = "code";
                options.Scope.Add("shoppinggateway.access");
                options.Scope.Add("mvcgateway.ordering.read");
                options.Scope.Add("mvcgateway.ordering.write");
                options.Scope.Add("mvcgateway.billing.read");
                options.Scope.Add("mvcgateway.billing.write");
                options.Scope.Add("mvcgateway.shipping.read");
                options.Scope.Add("mvcgateway.inventory.read");
                options.Scope.Add("mvcgateway.inventory.write");
                options.SaveTokens = true;
          
            });

            builder.Services.AddAccessTokenManagement();

            builder.Services.AddMvc();

            builder.Services.AddControllers();
     
            builder.Services.AddHttpContextAccessor();

     

            builder.Services.AddMicroStoreClinet()
                .AddUserAccessTokenHandler();

            return builder.Build();
        } 
    }
}
