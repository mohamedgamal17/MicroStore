using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class ShipmentListQueryModel : PagingAndSortingQueryParams
    {
        public string States { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }
}
