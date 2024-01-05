using Grpc.Core;
using MicroStore.BuildingBlocks.AspNetCore.Grpc;
using Newtonsoft.Json;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using FluentValidation.Results;

namespace MicroStore.BuildingBlocks.AspNetCore.Extensions
{
    public static class GrpcExtensions
    {
        public static GrpcError PreapreGrpcErrorResponse(this Exception exception)
        {
            return exception switch
            {

                BusinessException ex => GrpcError.BussniessLogicError(ex.Details),

                EntityNotFoundException ex => GrpcError.NotFoundError(ex.Message),

                _ => GrpcError.InternalError(exception.Message)
            };
        }

        public static void ThrowRpcException(this ValidationResult validationResult)
        {

            var validationErrros = validationResult.Errors.GroupBy(x => x.PropertyName)
                    .Select(x => new GrpcValidationError
                    {
                        Field = x.Key,
                        Errors = x.Select(x => x.ErrorMessage).ToArray()
                    }).ToList();


           var grpcError = GrpcError.ValidationErrors(validationErrros);

            grpcError.ThrowRpcException();
        }


        public static void ThrowRpcException(this Exception exception)
        {
            GrpcError errorResponse = exception.PreapreGrpcErrorResponse();

            errorResponse.ThrowRpcException();
        }


        public static void ThrowRpcException(this GrpcError errorResponse)
        {
            StatusCode statusCode = errorResponse.Code switch
            {
                GrpcErrorType.BussniessLogic => StatusCode.InvalidArgument,
                GrpcErrorType.ValidationError => StatusCode.InvalidArgument,
                GrpcErrorType.NotFound => StatusCode.NotFound,
                _ => StatusCode.Internal
            };


            Status status = new Status(statusCode, errorResponse.Title);

            var json =  JsonConvert.SerializeObject(errorResponse);

            Metadata metaData = new Metadata
            {
                { GrpcMetaDataConsts.Error ,json}
            };


            throw new RpcException(status, metaData);
        }

        public static GrpcError SerialzeGrpcError(this RpcException exception)
        {
            string json = exception.Trailers.GetValue(GrpcMetaDataConsts.Error)!;

            return JsonConvert.DeserializeObject<GrpcError>(json)!;
        }
    }
}
