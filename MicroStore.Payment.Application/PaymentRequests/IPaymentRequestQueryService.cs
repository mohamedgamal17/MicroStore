using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Domain.Shared.Dtos;
using Volo.Abp.Application.Services;
namespace MicroStore.Payment.Application.PaymentRequests
{
    public interface IPaymentRequestQueryService : IApplicationService
    {
        Task<UnitResult<PaymentRequestDto>> GetAsync(string paymentId, CancellationToken cancellationToken = default);
        Task<UnitResult<PaymentRequestDto>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
        Task<UnitResult<PaymentRequestDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
        Task<UnitResult<PagedResult<PaymentRequestListDto>>> ListPaymentAsync(PagingAndSortingQueryParams queryParams, string? userId = null, CancellationToken cancellationToken = default);

    }
}
