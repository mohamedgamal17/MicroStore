using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.Results;
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

                BusinessException ex => new BadRequestObjectResult(ex),

                EntityNotFoundException ex => new BadRequestObjectResult(ex),

                _ => new BadRequestObjectResult(exception),
            };

        };


        public static IActionResult ToOk<T>(this ResultV2<T> result )
        {
            return result.Match((r) =>
            {
                return new OkObjectResult(r);

            }, _exceptionHandler);
        }

        public static IActionResult ToCreatedAtAction<T>(this ResultV2<T> result, string? actionName = null, object? routeValues = null)
        {
            return result.Match(obj =>
            {
                return new CreatedAtActionResult(actionName, null , routeValues, obj);

            }, _exceptionHandler);
        }

        public static IActionResult ToCreatedAtAction<T>(this ResultV2<T> result, string? actionName = null, string? controllerName = null,object? routeValues = null)
        {
            return result.Match(obj =>
            {
                return new CreatedAtActionResult(actionName, controllerName, routeValues, obj);

            },_exceptionHandler);
        }

        public static IActionResult ToCreatedAtRoute<T>(this ResultV2<T> result, string? routeName = null, object? routeValues = null)
        {
            return result.Match((obj) =>
            {
                return new CreatedAtRouteResult(routeName, routeValues, obj);

            }, _exceptionHandler);
        }

        public static IActionResult ToAcceptedAtAction<T>(this ResultV2<T> result, string? actionName = null,  object? routeValues = null)
        {
            return result.Match(obj =>
            {
                return new AcceptedAtActionResult(actionName,null, routeValues, obj);

            }, _exceptionHandler);
        }

        public static IActionResult ToAcceptedAtAction<T>(this ResultV2<T> result, string? actionName = null, string? controllerName = null, object? routeValues = null)
        {
            return result.Match(obj =>
            {
                return new AcceptedAtActionResult(actionName, controllerName, routeValues, obj);

            }, _exceptionHandler);
        }

        public static IActionResult ToAcceptedAtRoute<T>(this ResultV2<T> result , string? routeName = null  , object? routeValues = null)
        {
            return result.Match((obj) =>
            {
                return new AcceptedAtRouteResult(routeName, routeValues, obj);

            }, _exceptionHandler);
        }


        public static IActionResult ToNoContent<T>(this ResultV2<T> result)
        {
            return result.Match((obj) =>
            {
                return new NoContentResult();

            }, _exceptionHandler);
        }

       
    }
}
