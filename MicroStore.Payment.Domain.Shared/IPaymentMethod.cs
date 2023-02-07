using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;

namespace MicroStore.Payment.Domain.Shared
{
    public interface IPaymentMethod
    {
        string PaymentGatewayName { get; }

        Task<ResponseResult<PaymentProcessResultDto>> Process(Guid paymentId, ProcessPaymentModel processPaymentModel, CancellationToken cancellationToken = default);

        Task<ResponseResult<PaymentRequestDto>> Complete(CompletePaymentModel completePaymentModel, CancellationToken cancellationToken = default);

        Task<ResponseResult<PaymentRequestDto>> Refund(Guid paymentId, CancellationToken cancellationToken = default);

    }
}
