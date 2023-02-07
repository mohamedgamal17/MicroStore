using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.BuildingBlocks.Results;
using System.Net;
using MicroStore.Payment.Domain.Shared;

namespace MicroStore.Payment.Application.Extensions
{
    public static class UnitResultExtensions
    {
        public static ResponseResult<T> ConvertFaildUnitResult<T>(this UnitResult unitResult)
        {
            if (unitResult.IsSuccess)
            {
                throw new InvalidOperationException();
            }

            var errorEnfo = new ErrorInfo
            {
                Message = unitResult.Error
            };

            if (unitResult.ErrorType == PaymentMethodErrorType.ValidationError
                || unitResult.ErrorType == PaymentMethodErrorType.BusinessLogicError)
            {
                return ResponseResult.Failure<T>((int)HttpStatusCode.BadRequest, errorEnfo);
            }

            if (unitResult.ErrorType == PaymentMethodErrorType.NotExist)
            {
                return ResponseResult.Failure<T>((int)HttpStatusCode.NotFound, errorEnfo);
            }

            return ResponseResult.Failure<T>((int)HttpStatusCode.BadRequest, errorEnfo);
        }
    }
}
