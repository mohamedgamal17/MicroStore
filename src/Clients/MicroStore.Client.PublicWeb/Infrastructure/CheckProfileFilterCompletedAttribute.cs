using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MicroStore.Client.PublicWeb.Consts;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;

namespace MicroStore.Client.PublicWeb.Infrastructure
{
    public class CheckProfilePageCompletedFilterAttribute : Attribute, IAsyncPageFilter
    {

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {

            if (context.HttpContext.User.Identity!.IsAuthenticated)
            {
                var userProfileService = context.HttpContext.RequestServices.GetRequiredService<UserProfileService>();
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<CheckProfilePageCompletedFilterAttribute>>();

                var userProfile = context.HttpContext.Items[HttpContextSharedItemsConsts.UserProfile] as User;

                if (userProfile == null)
                {

                    try
                    {
                        userProfile = await userProfileService.GetAsync();

                        logger.LogDebug("Retrived current user : {user}", userProfile.UserId);

                        context.HttpContext.Items.Add(HttpContextSharedItemsConsts.UserProfile, userProfile);

                        await next();
                    }
                    catch (MicroStoreClientException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        context.Result = new RedirectToPageResult("/Profile/Create", new { returnUrl = context.HttpContext.Request.Path });
                    }

                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }
    }

    public class CheckProfileActionCompletedFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity!.IsAuthenticated)
            {
                var userProfileService = context.HttpContext.RequestServices.GetRequiredService<UserProfileService>();
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<CheckProfileActionCompletedFilterAttribute>>();

                var userProfile = context.HttpContext.Items[HttpContextSharedItemsConsts.UserProfile] as User;

                if (userProfile == null)
                {

                    try
                    {
                        userProfile = await userProfileService.GetAsync();

                        logger.LogDebug("Retrived current user : {user}", userProfile.UserId);

                        context.HttpContext.Items.Add(HttpContextSharedItemsConsts.UserProfile, userProfile);

                        await next();
                    }
                    catch (MicroStoreClientException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        context.Result = new RedirectToPageResult("/Profile/Create", new { returnUrl = context.HttpContext.Request.Path });
                    }

                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
        }
    }
}
