using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.Ordering.Application.Models
{
    public class ProductSummaryReportModel : PagingQueryParams
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ReportPeriod Period { get; set; }
    }
}
