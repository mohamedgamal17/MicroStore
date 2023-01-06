using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.Payment.Application.Abstractions.Queries
{
    public class GetPaymentRequestListQuery :PagingAndSortingQueryParams ,IQuery
    {
    }
}
