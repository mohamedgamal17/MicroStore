using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;

namespace MicroStore.Payment.Domain.Shared
{
    public interface IPaymentMethod
    {
        string PaymentGatewayName { get; }

        Task<PaymentProcessResultDto> Process(Guid paymentId, ProcessPaymentModel processPaymentModel , CancellationToken cancellationToken = default);

        Task<PaymentRequestCompletedDto> Complete(CompletePaymentModel completePaymentModel, CancellationToken cancellationToken = default);

        Task Refund(Guid paymentId, CancellationToken cancellationToken = default);


    }
}
