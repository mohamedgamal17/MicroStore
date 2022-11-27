using MicroStore.Payment.Domain.Shared;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
namespace MicroStore.Payment.Application
{
    public class PaymentMethodResolver : IPaymentMethodResolver , ITransientDependency
    {
        private readonly IEnumerable<IPaymentMethod> _paymentMethods;

        public PaymentMethodResolver(IEnumerable<IPaymentMethod> paymentMethods)
        {
            _paymentMethods = paymentMethods;
        }

        public IPaymentMethod Resolve(string paymentGatewayName)
        {
            var paymentMethod = _paymentMethods.SingleOrDefault(x => x.PaymentGatewayName == paymentGatewayName);
            
            if(paymentMethod == null)
            {
                throw new EntityNotFoundException(typeof(IPaymentMethod));
            }

            return paymentMethod;
        }
    }
}
