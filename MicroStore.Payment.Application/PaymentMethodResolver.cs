using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Application.Abstractions;
using MicroStore.Payment.Application.Abstractions.Consts;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Payment.Application
{
    public class PaymentMethodResolver : IPaymentMethodResolver , ITransientDependency
    {
        private readonly IEnumerable<IPaymentMethod> _paymentMethods;

        public PaymentMethodResolver(IEnumerable<IPaymentMethod> paymentMethods)
        {
            _paymentMethods = paymentMethods;
        }

        public async Task<UnitResult<IPaymentMethod>> Resolve(string paymentGatewayName, CancellationToken cancellationToken = default)
        {
            var paymentMethod = _paymentMethods.SingleOrDefault(x => x.PaymentGatewayName == paymentGatewayName);
            
            if(paymentMethod == null)
            {
                 return UnitResult.Failure<IPaymentMethod>(PaymentMethodErrorType.NotExist,  $"Payment system with name :{paymentGatewayName}, is not exist" );
            }

            bool isEnabled = await paymentMethod.IsEnabled();

            if (!isEnabled)
            {
                return  UnitResult.Failure<IPaymentMethod>(PaymentMethodErrorType.BusinessLogicError, $"Payment system : {paymentGatewayName} is disabled"); ;
            }

            return UnitResult.Success(paymentMethod);
        }
    }
}
