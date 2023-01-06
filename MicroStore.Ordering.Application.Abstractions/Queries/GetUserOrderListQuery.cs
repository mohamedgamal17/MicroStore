using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging.Params;
namespace MicroStore.Ordering.Application.Abstractions.Queries
{
    public class GetUserOrderListQuery : PagingAndSortingQueryParams , IQuery
    {
        public string UserId { get; set; }
    }
}
