using MicroStore.BuildingBlocks.Utils.Paging.Params;

namespace MicroStore.Ordering.Application.Models
{
    public class CountrySalesReportModel : PagingQueryParams
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ReportPeriod Period { get; set; }

    }
}
