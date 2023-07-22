using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using MicroStore.Client.PublicWeb.Extensions;

namespace MicroStore.Client.PublicWeb.Controllers
{  
    public class AuthenticationController : Controller
    {
        public async Task Login(string? returnUrl )
        {
            if (HttpContext.User?.Identity?.IsAuthenticated ?? false)
            {
                
                //return RedirectToPage("Home");
            }

            var authenticationProperties = new AuthenticationProperties();

            authenticationProperties.RedirectUri = returnUrl ?? HttpContext.GetHostUrl();

            await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, authenticationProperties);

        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task Logout()
        {
            var authenticationProperties = new AuthenticationProperties();

            authenticationProperties.RedirectUri = HttpContext.GetHostUrl();

            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, authenticationProperties);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

    
    }
}
