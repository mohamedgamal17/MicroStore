using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MicroStore.Client.PublicWeb.Consts;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;

namespace MicroStore.Client.PublicWeb.Infrastructure
{
    public class CheckProfilePageCompletedFilterAttribute : Attribute, IAsyncPageFilter
    {
        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {

                var userProfile = context.HttpContext.Items[HttpContextSharedItemsConsts.UserProfile] as User;

                if (userProfile == null)
                {
                    context.Result = new RedirectToPageResult("/Profile/Create", new { returnUrl = context.HttpContext.Request.Path });
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

        public  Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
             return Task.CompletedTask;
        }
    }

    public class CheckProfileActionCompletedFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {

                var userProfile = context.HttpContext.Items[HttpContextSharedItemsConsts.UserProfile] as User;

                if (userProfile == null)
                {
                    context.Result = new RedirectToPageResult("/Profile/Create", new { returnUrl = context.HttpContext.Request.Path });
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
