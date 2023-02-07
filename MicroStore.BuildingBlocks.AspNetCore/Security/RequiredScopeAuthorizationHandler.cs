using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace MicroStore.BuildingBlocks.AspNetCore.Security
{
    [Authorize]
    public class RequiredScopeAuthorizationHandler : IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if(context.ActionDescriptor.GetType() != typeof(ControllerActionDescriptor))
            {
                return Task.CompletedTask; 
            }

            var attribute = context.ActionDescriptor.GetMethodInfo().GetCustomAttribute<RequiredScopeAttribute>();

            if (attribute == null || attribute?.AllowedScope == null)
            {
                return Task.CompletedTask;
            }

            var isUserAuthenticated = context.HttpContext.User.Identity?.IsAuthenticated ?? false;

            if (!isUserAuthenticated)
            {
                HandleUnAuthenticatedRequest(context);
            }

            var claim = context.HttpContext.User.Claims.Where(x => x.Type == "scope").FirstOrDefault();

            if (claim != null)
            {
                var scopes = claim.Value.Split(' ');

                if (scopes.Contains(attribute.AllowedScope))
                {
                    return Task.CompletedTask;
                }
            }
            return HandleUnAuthorizedRequest(context);
        }

        private Task HandleUnAuthorizedRequest(AuthorizationFilterContext context)
        {


            var details = new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
            };


            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status403Forbidden
            };

            return Task.CompletedTask;
        }


        private  Task HandleUnAuthenticatedRequest(AuthorizationFilterContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };

            return Task.CompletedTask;
        }
    }

     
}
