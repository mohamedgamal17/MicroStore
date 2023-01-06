using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging.Params;
namespace MicroStore.Ordering.Application.Abstractions.Queries
{
    public class GetOrderListQuery : PagingAndSortingQueryParams , IQuery
    {
    }
}
