using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
namespace MicroStore.Client.PublicWeb
{
    public static class HostExtensions
    {
        public static WebApplication ConfigureService(this WebApplicationBuilder builder)
        {
            var config = builder.Configuration;

            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

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
          
            });


            return builder.Build();
        } 
    }
}
