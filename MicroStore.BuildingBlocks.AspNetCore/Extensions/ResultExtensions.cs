using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.Utils.Results;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace MicroStore.BuildingBlocks.AspNetCore.Extensions
{
    public static  class ResultExtensions
    {
        private static readonly Func<Exception, IActionResult> _exceptionHandler = (exception) =>
        {

            return exception switch
            {
                AbpValidationException ex => new BadRequestObjectResult(exception),

                BusinessException ex => new BadRequestObjectResult(new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Invalid entity state",
                    Detail = ex.Message
                }),

                EntityNotFoundException ex =>  new NotFoundObjectResult (new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",

                    Title = "The specified resource was not found.",

                    Detail = ex.Message

                }),

                _ => new ObjectResult(new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.1",
                    Detail = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                },
            };

        };

        public static IActionResult ToFailure<T>(this Result<T> result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Result should be in failure state to preform this action");
            }

            return _exceptionHandler(result.Exception);
        }
        public static IActionResult ToOk<T>(this Result<T> result)
        {
            return result.Match((r) =>
            {
                return new OkObjectResult(r);

            }, _exceptionHandler);
        }

        public static IActionResult ToCreatedAtAction<T>(this Result<T> result, string? actionName = null, object? routeValues = null)
        {
            return result.Match(obj =>
            {
                return new CreatedAtActionResult(actionName, null , routeValues, obj);

            }, _exceptionHandler);
        }

        public static IActionResult ToCreatedAtAction<T>(this Result<T> result, string? actionName = null, string? controllerName = null,object? routeValues = null)
        {
            return result.Match(obj =>
            {
                return new CreatedAtActionResult(actionName, controllerName, routeValues, obj);

            },_exceptionHandler);
        }

        public static IActionResult ToCreatedAtRoute<T>(this Result<T> result, string? routeName = null, object? routeValues = null)
        {
            return result.Match((obj) =>
            {
                return new CreatedAtRouteResult(routeName, routeValues, obj);

            }, _exceptionHandler);
        }

        public static IActionResult ToAcceptedAtAction<T>(this Result<T> result, string? actionName = null,  object? routeValues = null)
        {
            return result.Match(obj =>
            {
                return new AcceptedAtActionResult(actionName,null, routeValues, obj);

            }, _exceptionHandler);
        }

        public static IActionResult ToAcceptedAtAction<T>(this Result<T> result, string? actionName = null, string? controllerName = null, object? routeValues = null)
        {
            return result.Match(obj =>
            {
                return new AcceptedAtActionResult(actionName, controllerName, routeValues, obj);

            }, _exceptionHandler);
        }

        public static IActionResult ToAcceptedAtRoute<T>(this Result<T> result , string? routeName = null  , object? routeValues = null)
        {
            return result.Match((obj) =>
            {
                return new AcceptedAtRouteResult(routeName, routeValues, obj);

            }, _exceptionHandler);
        }


        public static IActionResult ToNoContent<T>(this Result<T> result)
        {
            return result.Match((obj) =>
            {
                return new NoContentResult();

            }, _exceptionHandler);
        }

       
    }
}
