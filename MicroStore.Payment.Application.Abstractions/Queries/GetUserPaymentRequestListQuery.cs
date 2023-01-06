using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.Payment.Application.Abstractions.Queries
{
    public class GetUserPaymentRequestListQuery : PagingAndSortingQueryParams , IQuery
    {
        public string UserId { get; set; }
    }
}
