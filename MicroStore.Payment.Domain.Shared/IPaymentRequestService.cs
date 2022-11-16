using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Domain.Shared.Dtos;
namespace MicroStore.Payment.Domain.Shared
{
    public interface IPaymentRequestService
    {
           
        Task<Result<PaymentCompletedDto>> CompletePayment(string transactionId, string paymentGatewayapi, DateTime capturedDate);

        Task<Result<PaymentDto>> GetPayment(Guid paymentId);

        Task<Result<PaymentFaildDto>> SetPaymentFaild(Guid paymentId, string faultReason, DateTime faultDate);

        Task<Result<PaymentStartedDto>> StartPayment(Guid paymentId, string transactionId, string paymentGatewayapi, DateTime openedAt);
    }
}
