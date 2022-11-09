using MicroStore.Payment.Api.Dtos;
using MicroStore.Payment.Api.Models;

namespace MicroStore.Payment.Api.Services
{
    public interface IPaymentService
    {
        Task<PaymentDto> CreatePayment(CreatePaymentModel model);

        Task CapturePayment(string transactionId);

        Task RefundPayment(string transactionId);

    }
}
