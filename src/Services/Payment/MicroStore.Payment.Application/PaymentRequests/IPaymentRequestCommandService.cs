using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.Payment.Application.PaymentRequests
{
    public interface IPaymentRequestCommandService : IApplicationService
    {
        Task<Result<PaymentRequestDto>> CreateAsync(CreatePaymentRequestModel model, CancellationToken cancellationToken = default);
        Task<Result<PaymentProcessResultDto>> ProcessPaymentAsync(string paymentId, ProcessPaymentRequestModel model, CancellationToken cancellationToken = default);
        Task<Result<PaymentRequestDto>> CompleteAsync(CompletePaymentModel model, CancellationToken cancellationToken = default);
        Task<Result<PaymentRequestDto>> RefundPaymentAsync(string paymentId, CancellationToken cancellationToken = default);
    }
}
