using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Domain.Shared.Dtos;
using Volo.Abp.Application.Services;
namespace MicroStore.Payment.Application.PaymentRequests
{
    public interface IPaymentRequestQueryService : IApplicationService
    {
        Task<UnitResultV2<PaymentRequestDto>> GetAsync(string paymentId, CancellationToken cancellationToken = default);
        Task<UnitResultV2<PaymentRequestDto>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
        Task<UnitResultV2<PaymentRequestDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
        Task<UnitResultV2<PagedResult<PaymentRequestListDto>>> ListPaymentAsync(PagingAndSortingQueryParams queryParams, string? userId = null, CancellationToken cancellationToken = default);

    }
}
