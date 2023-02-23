using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;

namespace MicroStore.Payment.Domain.Shared
{
    public interface IPaymentMethod
    {
        string PaymentGatewayName { get; }

        Task<UnitResult<PaymentProcessResultDto>> Process(string paymentId, ProcessPaymentRequestModel processPaymentModel, CancellationToken cancellationToken = default);

        Task<UnitResult<PaymentRequestDto>> Refund(string paymentId, CancellationToken cancellationToken = default);

    }
}
