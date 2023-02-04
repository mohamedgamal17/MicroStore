using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using MicroStore.BuildingBlocks.Results.Http;
using System.Reflection;

namespace MicroStore.BuildingBlocks.AspNetCore.Security
{
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

            var claim = context.HttpContext.User.Claims.Where(x => x.Type == "scope").FirstOrDefault();

            if (claim != null)
            {
                var scopes = claim.Value.Split(' ');

                if (scopes.Contains(attribute.AllowedScope))
                {
                    return Task.CompletedTask;
                }
            }
            return HandleUnAuthorizedRequest(context,attribute);
        }

        private Task HandleUnAuthorizedRequest(AuthorizationFilterContext context, RequiredScopeAttribute attribute)
        {
            var request = context.HttpContext.Request;

            var message = new ErrorInfo
            {
                Type = "UnAuthorized",
                Message = $"You must have {attribute.AllowedScope} scope to preform this action"
            };

            
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

            context.Result = new ForbidResult();
       
            return Task.CompletedTask;
        }
    }

     
}
