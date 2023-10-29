using Microsoft.AspNetCore.Mvc.ModelBinding;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products
{
    public class ProductSalesReportModel : BasePagedListModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ReportPeriod GroupBy { get; set; }

        [BindNever]
        public List<ProductSalesReportVM> Data { get; set; } = new List<ProductSalesReportVM>();
    }
}
