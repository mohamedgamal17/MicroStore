using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using System.Net;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Client.PublicWeb.Infrastructure
{
    [ExposeServices(typeof(IUIExceptionHandler<MicroStoreClientException>))]
    public class MicroClientExceptionHandler : IUIExceptionHandler<MicroStoreClientException> , ITransientDependency
    {
        private readonly ILogger<MicroClientExceptionHandler> _logger;

        private Dictionary<HttpStatusCode, Func<HttpContext, MicroStoreClientException, Task>> _statusActionMapping;
        public MicroClientExceptionHandler(ILogger<MicroClientExceptionHandler> logger)
        {
            _logger = logger;
            _statusActionMapping = new()
            {
                 {HttpStatusCode.NotFound , HandleNotFound},
                 {HttpStatusCode.Unauthorized ,HandleUnauthorized },
                 {HttpStatusCode.Forbidden , HandleForbidden},
            };
        }



        public async Task HandleAsync(HttpContext context, MicroStoreClientException exception)
        {
            if (_statusActionMapping.ContainsKey(exception.StatusCode))
            {
                await _statusActionMapping[exception.StatusCode].Invoke(context, exception);
            }
            else
            {
                await HandleServerError(context, exception);
            }
        }


        private Task HandleNotFound(HttpContext context, MicroStoreClientException exception)
        {
            context.Response.Clear();

            context.Response.StatusCode = StatusCodes.Status404NotFound;

            return Task.CompletedTask;
        }

        private async Task HandleUnauthorized(HttpContext context, MicroStoreClientException exception)
        {
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            await context.ChallengeAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private async Task HandleForbidden(HttpContext context, MicroStoreClientException exception)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            await context.ForbidAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        }


        private Task HandleServerError(HttpContext context, MicroStoreClientException exception)
        {
            _logger.LogException(exception, LogLevel.Warning);

            context.Response.Clear();

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return Task.CompletedTask;
        }


    }
}
