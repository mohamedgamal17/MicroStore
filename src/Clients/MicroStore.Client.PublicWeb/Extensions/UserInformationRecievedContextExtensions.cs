using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using MicroStore.AspNetCore.UI;
using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;
using System.Security.Claims;

namespace MicroStore.Client.PublicWeb.Extensions
{
    public static class UserInformationRecievedContextExtensions
    {


        public static async Task MigrateUserBasketAsync(this UserInformationReceivedContext contxt)
        {
            var workContext = contxt.HttpContext.RequestServices.GetRequiredService<IWorkContext>();

            var basketService = contxt.HttpContext.RequestServices.GetRequiredService<BasketService>();

            var userClaims = contxt.Principal.Claims;

            var userId = contxt.Principal.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var requestOptions = new BasketMigrateRequestOptions
            {
                FromUserId = workContext.TryToGetCurrentUserId(),
                ToUserId = userId
            };

            var requestHeader = new RequestHeaderOptions
            {
                Authorization = contxt.ProtocolMessage.AccessToken
            };

            await basketService.MigrateAsync(requestOptions, requestHeader);

            workContext.ClearAnonymousUserCookie();
        }

    }
}
