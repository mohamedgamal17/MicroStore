using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Models;

namespace MicroStore.Payment.Application.Abstractions
{
    public interface IPaymentMethod
    {
        string PaymentGatewayName { get; }

        Task<PaymentProcessResultDto> Process(Guid paymentId, ProcessPaymentModel processPaymentModel, CancellationToken cancellationToken = default);

        Task<PaymentRequestCompletedDto> Complete(CompletePaymentModel completePaymentModel, CancellationToken cancellationToken = default);

        Task Refund(Guid paymentId, CancellationToken cancellationToken = default);

        Task<bool> IsEnabled();

    }
}
