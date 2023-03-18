using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.Payment.Application.PaymentRequests
{
    public interface IPaymentRequestCommandService : IApplicationService
    {
        Task<ResultV2<PaymentRequestDto>> CreateAsync(CreatePaymentRequestModel model, CancellationToken cancellationToken = default);
        Task<ResultV2<PaymentProcessResultDto>> ProcessPaymentAsync(string paymentId, ProcessPaymentRequestModel model, CancellationToken cancellationToken = default);
        Task<ResultV2<PaymentRequestDto>> RefundPaymentAsync(string paymentId, CancellationToken cancellationToken = default);
    }
}
