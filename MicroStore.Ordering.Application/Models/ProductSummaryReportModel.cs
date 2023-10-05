namespace MicroStore.Ordering.Application.Models
{
    public class ProductSummaryReportModel
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ReportPeriod Period { get; set; }
    }
}
