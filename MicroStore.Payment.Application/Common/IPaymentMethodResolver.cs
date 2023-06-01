using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Domain.Shared;

namespace MicroStore.Payment.Application.Common
{
    public interface IPaymentMethodResolver
    {
        Task<Result<IPaymentMethod>> Resolve(string paymentGatewayName, CancellationToken cancellationToken = default);
    }
}
