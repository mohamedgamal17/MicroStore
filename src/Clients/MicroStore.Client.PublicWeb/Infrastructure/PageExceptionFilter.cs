using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using System.Net;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http;

namespace MicroStore.Client.PublicWeb.Infrastructure
{
    public class PageExceptionFilter : IAsyncPageFilter
    {
        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }
        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (context.HandlerMethod == null || !ShouldHandleException(context))
            {
                await next();

                return;
            }

            var pageHandlerExecutedContext = await next();

            if (pageHandlerExecutedContext.Exception == null)
            {
                return;
            }

            await HandleExpcetion(pageHandlerExecutedContext);
        }

        public bool ShouldHandleException(PageHandlerExecutingContext context)
        {

            if (context.ActionDescriptor.IsPageAction() && 
                ActionResultHelper.IsObjectResult(context.HandlerMethod!.MethodInfo.ReturnType, typeof(void)))
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

        private void LogException(PageHandlerExecutedContext context, LogLevel logLevel = LogLevel.Error)
        {
            var logger = context.GetRequiredService<ILogger<ExceptionFilter>>();

            logger.LogInformation("Excp handling from filter");

            logger.LogException(context.Exception);
        }
        private async Task HandleExpcetion(PageHandlerExecutedContext context)
        {
            var exception = context.Exception;

            if (exception is MicroStoreClientException clientException)
            {
                if (clientException.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (context.HttpContext.User.Identity?.IsAuthenticated ?? false)
                    {
                        await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    }

                    await context.HttpContext.ChallengeAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
                else if (clientException.StatusCode == HttpStatusCode.Forbidden)
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
