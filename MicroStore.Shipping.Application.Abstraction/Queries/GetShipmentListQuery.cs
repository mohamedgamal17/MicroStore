using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.Shipping.Application.Abstraction.Queries
{
    public class GetShipmentListQuery : PagingQueryParams , IQuery
    {
    }
}
