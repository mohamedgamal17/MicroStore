using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.Payment.Application.PaymentRequests
{
    public interface IPaymentRequestQueryService : IApplicationService
    {
        Task<Result<PaymentRequestDto>> GetAsync(string paymentId, CancellationToken cancellationToken = default);
        Task<Result<PaymentRequestDto>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
        Task<Result<PaymentRequestDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<PaymentRequestDto>>> ListPaymentAsync(PaymentRequestListQueryModel queryParams, string? userId = null, CancellationToken cancellationToken = default);

        Task<Result<List<PaymentRequestDto>>> ListPaymentByOrderIdsAsync(List<string> orderIds, CancellationToken cancellationToken = default);

        Task<Result<List<PaymentRequestDto>>> ListPaymentByOrderNumbersAsync(List<string> orderNumbers, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<PaymentRequestDto>>> SearchByOrderNumber(PaymentRequestSearchModel model , CancellationToken cancellationToken = default);

    }
}
