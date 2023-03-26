using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
namespace MicroStore.Client.PublicWeb.Controllers
{  
    public class AuthenticationController : Controller
    {
        public async Task Login( string? returnUrl )
        {

            var authenticationProperties = new AuthenticationProperties();

            authenticationProperties.RedirectUri = returnUrl ?? HttpContext.Request.Path;

            await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, authenticationProperties);

        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task Logout()
        {
            var authenticationProperties = new AuthenticationProperties();

            authenticationProperties.RedirectUri = HttpContext.Request.Path;

            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, authenticationProperties);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

    
    }
}
