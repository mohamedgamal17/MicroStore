using Microsoft.AspNetCore.Mvc.ModelBinding;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Geographic
{
    public class CountrySalesReportModel : BasePagedListModel
    {
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public ReportPeriod GroupBy { get; set; }

        [BindNever]
        public List<CountrySalesReportVM> Data { get; set; }
    }
}
