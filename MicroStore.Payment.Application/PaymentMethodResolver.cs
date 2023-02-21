using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.Abstractions;
using MicroStore.Payment.Domain;
using MicroStore.Payment.Domain.Shared;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Payment.Application
{
    public class PaymentMethodResolver : IPaymentMethodResolver , ITransientDependency
    {
        private readonly IEnumerable<IPaymentMethod> _paymentMethods;

        private readonly IRepository<PaymentSystem> _paymentSystemRepository;
        public PaymentMethodResolver(IEnumerable<IPaymentMethod> paymentMethods, IRepository<PaymentSystem> paymentSystemRepository)
        {
            _paymentMethods = paymentMethods;
            _paymentSystemRepository = paymentSystemRepository;
        }

        public async Task<UnitResultV2<IPaymentMethod>> Resolve(string paymentGatewayName, CancellationToken cancellationToken = default)
        {
            var paymentMethod = _paymentMethods.SingleOrDefault(x => x.PaymentGatewayName == paymentGatewayName);
            
            if(paymentMethod == null)
            {
                 return UnitResultV2.Failure<IPaymentMethod>(ErrorInfo.NotFound( $"Payment system with name :{paymentGatewayName}, is not exist") );
            }

            bool isEnabled = await IsPaymentSystemEnabled(paymentGatewayName, cancellationToken);

            if (!isEnabled)
            {
                return UnitResultV2.Failure<IPaymentMethod>( ErrorInfo.BusinessLogic($"Payment system : {paymentGatewayName} is disabled")); ;
            }

            return UnitResultV2.Success(paymentMethod);
        }

        private async Task<bool> IsPaymentSystemEnabled(string systemName , CancellationToken cancellationToken = default)
        {
            var paymentSystem = await _paymentSystemRepository.SingleAsync(x => x.Name == systemName, cancellationToken);

            return paymentSystem.IsEnabled;
        }
    }
}
