using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Models;

namespace MicroStore.Payment.Application.Abstractions
{
    public interface IPaymentMethod
    {
        string PaymentGatewayName { get; }

        Task<ResponseResult> Process(Guid paymentId, ProcessPaymentModel processPaymentModel, CancellationToken cancellationToken = default);

        Task<ResponseResult> Complete(CompletePaymentModel completePaymentModel, CancellationToken cancellationToken = default);

        Task<ResponseResult> Refund(Guid paymentId, CancellationToken cancellationToken = default);

        Task<bool> IsEnabled();

    }
}
