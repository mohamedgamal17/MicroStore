using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroStore.Client.PublicWeb.Controllers
{
    [Authorize]
    public class TokenDebugController : Controller
    {
        private readonly ILogger<TokenDebugController> logger;

        public TokenDebugController(ILogger<TokenDebugController> logger)
        {
            this.logger = logger;
        }

        public async Task<IActionResult> DebugIdentityToken()
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("Id token : {token}", await HttpContext.GetTokenAsync("id_token"));
            }

            return Ok();
        }

        public async Task<IActionResult> DebugAccessToken()
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("Access token : {token}", await HttpContext.GetTokenAsync("access_token"));
            }

            return Ok();
        }

        public  IActionResult DebugClaims()
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("User Claims : {claims}", HttpContext.User.Claims);
            }


            return Ok();
        }
    }
}
