using MicroStore.BuildingBlocks.Utils.Paging.Params;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class ShipmentListQueryModel : PagingAndSortingQueryParams
    {
        public string OrderNumber { get; set; }
        public string TrackingNumber { get; set; }
        public int? Status { get; set; }
        public string Country { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
