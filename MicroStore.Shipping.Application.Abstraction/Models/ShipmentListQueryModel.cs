using MicroStore.BuildingBlocks.Utils.Paging.Params;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class ShipmentListQueryModel : PagingAndSortingQueryParams
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string TrackingNumber { get; set; } = string.Empty;
        public int Status { get; set; } = -1;
        public string Country { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.MinValue;
        public DateTime EndDate { get; set; } = DateTime.MinValue;
    }
}
