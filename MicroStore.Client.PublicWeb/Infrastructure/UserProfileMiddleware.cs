using MicroStore.Client.PublicWeb.Consts;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;
using System.Security.Claims;

namespace MicroStore.Client.PublicWeb.Infrastructure
{
    public class UserProfileMiddleware
    {
        private readonly UserProfileService _userPorfileService;
        private readonly RequestDelegate _next;
        private readonly ILogger<UserProfileMiddleware> _logger;

        public UserProfileMiddleware(UserProfileService userPorfileService, RequestDelegate next, ILogger<UserProfileMiddleware> logger)
        {
            _userPorfileService = userPorfileService;
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if(context.User.Identity.IsAuthenticated)
            {

                try
                {
                    var profileRetrived = (context.Items[HttpContextSharedItemsConsts.UserProfile] as User) != null;


                    if (!profileRetrived)
                    {
                        var userProfile = await _userPorfileService.GetAsync();

                        _logger.LogDebug("Retrived current user : {user}", userProfile.UserId);

                        context.Items.Add(HttpContextSharedItemsConsts.UserProfile, userProfile);
                    }
                  
                }
                catch (MicroStoreClientException ex)
                    when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogDebug("Current user : {user}, has not created his profile yet", context.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
                }
            }

            await _next(context);
        }
    }
}
