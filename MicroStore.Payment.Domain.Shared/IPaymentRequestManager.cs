using MicroStore.Payment.Domain.Shared.Dtos;
namespace MicroStore.Payment.Domain.Shared
{
    public interface IPaymentRequestManager
    {
        Task<PaymentRequestDto> GetPaymentRequest(string paymentId, CancellationToken cancellationToken = default);
        Task<PaymentRequestDto> Complete(string paymentId , string paymentGateway, string transactionId, DateTime capturedAt, CancellationToken cancellationToken = default );
        Task<PaymentRequestDto> Refund(string paymentId, DateTime refundedAt, string? description = null, CancellationToken cancellationToken = default);
        Task<PaymentRequestDto> MarkAsFaild(string paymentId, string paymentGateway, DateTime faultAt, CancellationToken cancellationToken = default);

    }
}
