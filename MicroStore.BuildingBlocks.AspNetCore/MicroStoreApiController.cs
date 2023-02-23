using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using System.Net;
using Volo.Abp.AspNetCore.Mvc;

namespace MicroStore.BuildingBlocks.AspNetCore
{
    public class MicroStoreApiController : AbpControllerBase
    {

        [NonAction]
        public IActionResult FromResult<T>(ResponseResult<T> result)
        {
            if (result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.EnvelopeResult.Result);
            }

            return StatusCode(result.StatusCode, result.EnvelopeResult.Error);
        }



        protected IActionResult FromResultV2<T>(UnitResultV2<T> result , HttpStatusCode successStatusCode)
        {
            if (result.IsSuccess)
            {
                return StatusCode((int)successStatusCode, result.Result);
            }

            return ConvertErrorResult(result);
        }

        protected IActionResult FromResultV2(UnitResultV2 result , HttpStatusCode successStatusCode)
        {
            if (result.IsSuccess)
            {
                return StatusCode((int)successStatusCode);
            }

            return ConvertErrorResult(result);
        }


        [NonAction]
        protected IActionResult ConvertErrorResult(UnitResultV2 result)
        {
            switch (result.Error.Type)
            {
                case HttpErrorType.NotFoundError:
                    return NotFound(new ProblemDetails
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        Title = "The specified resource was not found.",
                        Detail = result.Error.Message
                    });

                case HttpErrorType.ValidationError:
                    return BadRequest(new ValidationProblemDetails(result.Error.Errors)
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",

                    });

                case HttpErrorType.BusinessLogicError:
                    return BadRequest(new ProblemDetails
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        Title = "Invalid entity state",
                        Detail = result.Error.Message
                    });

                case HttpErrorType.UnAuthenticatedError:
                    return StatusCode(StatusCodes.Status401Unauthorized, new ProblemDetails
                    {
                        Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                        Title = "Unauthorized",
                    });

                case HttpErrorType.UnAuthorizedError:
                    return StatusCode(StatusCodes.Status403Forbidden, new ProblemDetails
                    {
                        Title = "Forbidden",
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
                    });
                case HttpErrorType.BadGatewayError:
                    return StatusCode(StatusCodes.Status502BadGateway, new ProblemDetails
                    {
                        Title = "BadGateway",
                        Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.3",
                        Detail = result.Error.Message
                    });

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.1",
                        Detail = result.Error.Message
                    });

            }
        }
     
    }
}