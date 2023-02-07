using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Domain.Shared.Dtos;

namespace MicroStore.Payment.Domain.Shared
{
    public interface IPaymentRequestManager
    {
        Task<PaymentRequestDto> GetPaymentRequest(Guid paymentId, CancellationToken cancellationToken = default);
        Task<PaymentRequestDto> Complete(Guid paymentId , string paymentGateway, string transactionId, DateTime capturedAt, CancellationToken cancellationToken = default );
        Task<PaymentRequestDto> Refund(Guid paymentId, DateTime refundedAt, string? description = null, CancellationToken cancellationToken = default);
        Task<PaymentRequestDto> MarkAsFaild(Guid paymentId, string paymentGateway, DateTime faultAt, CancellationToken cancellationToken = default);

    }
}
