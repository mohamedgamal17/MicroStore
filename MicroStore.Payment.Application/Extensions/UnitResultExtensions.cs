using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.BuildingBlocks.Results;
using System.Net;
using MicroStore.Payment.Application.Abstractions.Consts;

namespace MicroStore.Payment.Application.Extensions
{
    public static class UnitResultExtensions
    {
        public static ResponseResult ConvertFaildUnitResult(this UnitResult unitResult)
        {
            if (unitResult.IsSuccess)
            {
                throw new InvalidOperationException();
            }

            var errorEnfo = new ErrorInfo
            {
                Message = unitResult.Error
            };



            if (unitResult.ErrorType ==
                PaymentMethodErrorType.ValidationError)
            {
                return ResponseResult.Failure((int)HttpStatusCode.BadRequest, errorEnfo);
            }

            if (unitResult.ErrorType == PaymentMethodErrorType.NotExist)
            {
                return ResponseResult.Failure((int)HttpStatusCode.NotFound, errorEnfo);
            }

            return ResponseResult.Failure((int)HttpStatusCode.BadRequest, errorEnfo);
        }
    }
}
