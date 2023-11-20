using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using System.Net;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http;

namespace MicroStore.Client.PublicWeb.Infrastructure
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!ShouldHandleException(context))
            {
                LogException(context);
                return;
            }

            await HandleExpcetion(context);
        }

        public bool ShouldHandleException(ExceptionContext context)
        {
            if(context.ActionDescriptor.IsControllerAction()
                && context.ActionDescriptor.HasObjectResult())

            {
                return true;

            }

            if (context.HttpContext.Request.CanAccept(MimeTypes.Application.Json))
            {
                return true;
            }

            if (context.HttpContext.Request.IsAjax())
            {
                return true;
            }

            return false;
        }


        private void LogException(ExceptionContext context, LogLevel logLevel = LogLevel.Error)
        {
            var logger = context.GetRequiredService<ILogger<ExceptionFilter>>();

            logger.LogException(context.Exception);
        }
        private async Task HandleExpcetion(ExceptionContext context)
        {
            var exception = context.Exception;

            if(exception is MicroStoreClientException clientException)
            {
                if(clientException.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (context.HttpContext.User.Identity?.IsAuthenticated ?? false)
                    {
                        await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    }

                    await context.HttpContext.ChallengeAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
                else if(clientException.StatusCode == HttpStatusCode.Forbidden)
                {
                    await context.HttpContext.ForbidAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
                else
                {
                    LogException(context, LogLevel.Warning);

                    var remoteServiceErrorInfo = ConvertMicroStoreClientExceptionToRemoteServiceError(clientException);

                    context.HttpContext.Response.StatusCode = (int)clientException.StatusCode;

                    context.Result = new ObjectResult(remoteServiceErrorInfo);
                }         
            }
            else
            {
                LogException(context, LogLevel.Error);

                var remoteServiceErrorInfo = new RemoteServiceErrorInfo(exception.Message);

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                context.Result = new ObjectResult(remoteServiceErrorInfo);
            }

            context.Exception = null;
        }

        private RemoteServiceErrorInfo ConvertMicroStoreClientExceptionToRemoteServiceError(MicroStoreClientException exception)
        {
            var remoteServiceErrorInfo = new RemoteServiceErrorInfo
            {
                Message = exception.Error.Title,
                Details = exception.Error.Detail,
                ValidationErrors = exception.Error.Errors?
                .Select((kvp) => new RemoteServiceValidationErrorInfo
                {
                    Members = new string[] { kvp.Key },
                    Message = kvp.Value.JoinAsString(" ")
                }).ToArray()
            };

            return remoteServiceErrorInfo;
        }
    }
}
