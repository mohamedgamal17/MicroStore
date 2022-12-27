using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Const;
using System.Net;

namespace MicroStore.Shipping.Application.Extensions
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



            if(unitResult.ErrorType == 
                ShippingSystemErrorType.ValidationError)
            {
                return ResponseResult.Failure((int)HttpStatusCode.BadRequest,errorEnfo);
            }

            if(unitResult.ErrorType == ShippingSystemErrorType.NotExist)
            {
                return ResponseResult.Failure((int)HttpStatusCode.NotFound, errorEnfo);
            }

            return ResponseResult.Failure((int)HttpStatusCode.BadRequest, errorEnfo);
        }
    }
}
