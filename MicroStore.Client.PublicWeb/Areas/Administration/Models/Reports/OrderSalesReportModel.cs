using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Reports
{
    public class OrderSalesReportModel : BasePagedListModel
    {
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public ReportPeriod GroupBy { get; set; }
        public List<OrderSalesReportVM> Data { get; set; }

    }
}
