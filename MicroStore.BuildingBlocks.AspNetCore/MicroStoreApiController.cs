using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using Volo.Abp.AspNetCore.Mvc;
namespace MicroStore.BuildingBlocks.AspNetCore
{
    public class MicroStoreApiController : AbpController
    {


        protected async Task<ValidationResult> ValidateModel<TModel>(TModel model)
        {
            var validator = ResolveValidator<TModel>();

            if (validator == null) return new ValidationResult();

            var result =  await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
            }

            return result;
        }

        protected IActionResult InvalideModelState()
        {

            var details = new ValidationProblemDetails(ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            return new BadRequestObjectResult(details);
        }


        private IValidator<T>? ResolveValidator<T>()
        {
            return LazyServiceProvider.LazyGetService<IValidator<T>> ();   
        }

        //[NonAction]
        //protected IActionResult ConvertErrorResult(UnitResult result)
        //{
        //    switch (result.Error.Type)
        //    {
        //        case HttpErrorType.NotFoundError:
        //            return NotFound(new ProblemDetails
        //            {
        //                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        //                Title = "The specified resource was not found.",
        //                Detail = result.Error.Message
        //            });

        //        case HttpErrorType.ValidationError:
        //            return BadRequest(new ValidationProblemDetails(result.Error.Errors)
        //            {
        //                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",

        //            });

        //        case HttpErrorType.BusinessLogicError:
        //            return BadRequest(new ProblemDetails
        //            {
        //                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        //                Title = "Invalid entity state",
        //                Detail = result.Error.Message
        //            });

        //        case HttpErrorType.UnAuthenticatedError:
        //            return StatusCode(StatusCodes.Status401Unauthorized, new ProblemDetails
        //            {
        //                Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
        //                Title = "Unauthorized",
        //            });

        //        case HttpErrorType.UnAuthorizedError:
        //            return StatusCode(StatusCodes.Status403Forbidden, new ProblemDetails
        //            {
        //                Title = "Forbidden",
        //                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
        //            });
        //        case HttpErrorType.BadGatewayError:
        //            return StatusCode(StatusCodes.Status502BadGateway, new ProblemDetails
        //            {
        //                Title = "BadGateway",
        //                Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.3",
        //                Detail = result.Error.Message
        //            });

        //        default:
        //            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
        //            {
        //                Title = "Internal Server Error",
        //                Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.1",
        //                Detail = result.Error.Message
        //            });

        ////    }
        //}
     
    }
}