using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Payment.Application.Common;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Domain.Shared.Configuration;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
namespace MicroStore.Payment.Application
{


    public class PaymentMethodProviderResolver : IPaymentMethodProviderResolver , ITransientDependency
    {
        private readonly PaymentSystemOptions _options;

        private readonly IServiceProvider _serviceProvider;
        public PaymentMethodProviderResolver(IOptions<PaymentSystemOptions> options, IServiceProvider serviceProvider)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }

        public async Task<Result<IPaymentMethodProvider>> Resolve(string name, CancellationToken cancellationToken = default)
        {
            var system = _options.Systems.SingleOrDefault(x => x.Name == name);

            if (system == null)
            {
                return new Result<IPaymentMethodProvider>(new EntityNotFoundException($"Payment system with name :{name}, is not exist"));
            }

            if (!system.IsEnabled)
            {
                return new Result<IPaymentMethodProvider>(new BusinessException($"Payment system : {name} is disabled")); ;
            }

            var provider = (IPaymentMethodProvider) _serviceProvider.GetRequiredService(system.Provider);

            if(provider ==  null)
            {
                throw new InvalidOperationException($"Payment method provider must implement {typeof(IPaymentMethodProvider).AssemblyQualifiedName}");
            }

            return new Result<IPaymentMethodProvider>(provider);
        }
    }
}
