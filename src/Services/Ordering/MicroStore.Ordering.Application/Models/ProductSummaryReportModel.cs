using MicroStore.BuildingBlocks.Utils.Paging.Params;

namespace MicroStore.Ordering.Application.Models
{
    public class ProductSummaryReportModel : PagingQueryParams
    {
        public DateTime StartDate { get; set; } = DateTime.MinValue;

        public DateTime EndDate { get; set; } = DateTime.MinValue;

        public ReportPeriod Period { get; set; }
    }
}
