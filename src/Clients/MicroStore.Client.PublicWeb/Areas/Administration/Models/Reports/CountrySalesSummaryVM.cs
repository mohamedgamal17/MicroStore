namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Reports
{
    public class CountrySalesSummaryVM
    {
        public string CountryCode { get; set; }
        public double TotalTaxPrice { get; set; }
        public double TotalShippingPrice { get; set; }
        public double TotalPrice { get; set; }
        public long TotalOrders { get; set; }
    }
}
