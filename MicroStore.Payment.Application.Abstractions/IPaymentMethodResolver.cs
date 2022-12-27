using MicroStore.BuildingBlocks.Results;

namespace MicroStore.Payment.Application.Abstractions
{
    public interface IPaymentMethodResolver
    {
        Task<UnitResult<IPaymentMethod>> Resolve(string paymentGatewayName, CancellationToken cancellationToken  = default);
    }
}
