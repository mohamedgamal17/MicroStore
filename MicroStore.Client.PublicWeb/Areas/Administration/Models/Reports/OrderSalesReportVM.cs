namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Reports
{
    public class OrderSalesReportVM
    {
        public long TotalOrders { get; set; }
        public double TotalShippingPrice { get; set; }
        public double TotalTaxPrice { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
