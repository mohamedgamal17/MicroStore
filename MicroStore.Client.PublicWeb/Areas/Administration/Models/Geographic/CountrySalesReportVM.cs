namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Geographic
{
    public class CountrySalesReportVM
    {
        public double TotalTaxPrice { get; set; }
        public double TotalShippingPrice { get; set; }
        public double TotalPrice { get; set; }
        public long TotalOrders { get; set; }
        public DateTime Date { get; set; }
    }
}
