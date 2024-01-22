using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.AspNetCore.Grpc;
using System.Net.NetworkInformation;

namespace MicroStore.Bff.Shopping.Filters
{
    public class ExceptionHandlerFilter : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
           await HandleGrpcException(context);
        }


        private async Task HandleGrpcException(ExceptionContext context)
        {
            RpcException rpcException = (RpcException)context.Exception;

            ProblemDetails problemDetails;
            int httpStatusCode = 0;
            StatusCode rpcStatusCode = rpcException.StatusCode;

            if(rpcStatusCode == StatusCode.Unauthenticated)
            {
                problemDetails = new ProblemDetails
                {
                    Title = "Unauthorized",
                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                    Status = StatusCodes.Status403Forbidden
                };

                httpStatusCode = StatusCodes.Status401Unauthorized;
            }
            else if(rpcStatusCode == StatusCode.PermissionDenied)
            {
                problemDetails = new ProblemDetails
                {
                    Title = "Forbidden",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                    Status=  StatusCodes.Status403Forbidden
                };

                httpStatusCode = StatusCodes.Status403Forbidden;

            }
            else if(rpcStatusCode == StatusCode.InvalidArgument 
                || rpcStatusCode == StatusCode.NotFound)

            {
                problemDetails = MapGrpcErrorToProblemDetails(rpcException.SerialzeGrpcError());

                httpStatusCode = rpcStatusCode == StatusCode.NotFound ? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest;
            }
            else
            {
                problemDetails = new ProblemDetails
                {
                    Title = "Bad Gateway",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.3",
                    Status = StatusCodes.Status502BadGateway
                };
                httpStatusCode = StatusCodes.Status502BadGateway;
            }

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = httpStatusCode
            };

            context.ExceptionHandled = true;
        }

        private HttpValidationProblemDetails MapGrpcErrorToProblemDetails(GrpcError grpcError)
        {

            HttpValidationProblemDetails problemDetails = new HttpValidationProblemDetails();

            problemDetails.Type = grpcError.Code switch
            {
                GrpcErrorType.BussniessLogic => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                GrpcErrorType.ValidationError => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                GrpcErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                _ => "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.1"
            };

            problemDetails.Title = grpcError.Title;

            problemDetails.Detail = grpcError.Details;

            problemDetails.Status = MapGrpcStatusCode(grpcError);

            if (grpcError.Errors != null && grpcError.Errors.Count > 1)
            {
                grpcError.Errors
                .ForEach(x => problemDetails.Errors.Add(x.Field, x.Errors));
            }

            return problemDetails;
        }

        private int MapGrpcStatusCode(GrpcError grpcError)
        {
            return grpcError.Code switch
            {
                GrpcErrorType.BussniessLogic => StatusCodes.Status400BadRequest,
                GrpcErrorType.ValidationError => StatusCodes.Status400BadRequest,
                GrpcErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
        } 
    }
}
