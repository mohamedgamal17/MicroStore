using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Payment.Application.Abstractions.Dtos;

namespace MicroStore.Payment.Application.Abstractions.Queries
{
    public class GetUserPaymentRequestListQuery : PagingAndSortingQueryParams , IQuery<PagedResult<PaymentRequestListDto>>
    {
        public string UserId { get; set; }
    }
}
