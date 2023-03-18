using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Domain.Shared.Dtos;
using Volo.Abp.Application.Services;
namespace MicroStore.Payment.Application.PaymentRequests
{
    public interface IPaymentRequestQueryService : IApplicationService
    {
        Task<Result<PaymentRequestDto>> GetAsync(string paymentId, CancellationToken cancellationToken = default);
        Task<Result<PaymentRequestDto>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
        Task<Result<PaymentRequestDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<PaymentRequestListDto>>> ListPaymentAsync(PagingAndSortingQueryParams queryParams, string? userId = null, CancellationToken cancellationToken = default);

    }
}
