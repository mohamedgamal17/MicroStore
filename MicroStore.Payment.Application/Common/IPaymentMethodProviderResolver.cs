using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Domain.Shared;

namespace MicroStore.Payment.Application.Common
{
    public interface IPaymentMethodProviderResolver
    {
        Task<Result<IPaymentMethodProvider>> Resolve(string paymentGatewayName, CancellationToken cancellationToken = default);
    }
}
