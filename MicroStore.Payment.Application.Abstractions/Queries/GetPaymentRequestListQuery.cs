using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Payment.Application.Abstractions.Dtos;

namespace MicroStore.Payment.Application.Abstractions.Queries
{
    public class GetPaymentRequestListQuery :PagingAndSortingQueryParams ,IQuery<PagedResult<PaymentRequestListDto>>
    {
    }
}
