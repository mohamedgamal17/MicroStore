using AutoMapper;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Reports;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Mappers.Reports
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<OrderSalesReport, OrderSalesReportVM>();
        }
    }
}
