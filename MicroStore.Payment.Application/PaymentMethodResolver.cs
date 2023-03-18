using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.Abstractions;
using MicroStore.Payment.Domain;
using MicroStore.Payment.Domain.Shared;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
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

        public async Task<ResultV2<IPaymentMethod>> Resolve(string paymentGatewayName, CancellationToken cancellationToken = default)
        {
            var paymentMethod = _paymentMethods.SingleOrDefault(x => x.PaymentGatewayName == paymentGatewayName);
            
            if(paymentMethod == null)
            {
                 return new ResultV2<IPaymentMethod>(new EntityNotFoundException( $"Payment system with name :{paymentGatewayName}, is not exist") );
            }

            bool isEnabled = await IsPaymentSystemEnabled(paymentGatewayName, cancellationToken);

            if (!isEnabled)
            {
                return new ResultV2<IPaymentMethod>( new BusinessException($"Payment system : {paymentGatewayName} is disabled")); ;
            }

            return new ResultV2<IPaymentMethod>(paymentMethod);
        }

        private async Task<bool> IsPaymentSystemEnabled(string systemName , CancellationToken cancellationToken = default)
        {
            var paymentSystem = await _paymentSystemRepository.SingleAsync(x => x.Name == systemName, cancellationToken);

            return paymentSystem.IsEnabled;
        }
    }
}
