using MicroStore.BuildingBlocks.Utils.Paging.Params;

namespace MicroStore.Ordering.Application.Models
{
    public class OrderSalesReportModel : PagingQueryParams
    {
        public DateTime StartDate { get; set; } = DateTime.MinValue;
        public DateTime EndDate { get; set; } = DateTime.MinValue;
        public ReportPeriod Period { get; set; }
        public string Status { get; set; }
    }
}
