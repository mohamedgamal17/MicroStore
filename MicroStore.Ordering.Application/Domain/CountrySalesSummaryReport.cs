namespace MicroStore.Ordering.Application.Domain
{
    public class CountrySalesSummaryReport
    {
        public string CountryCode { get; set; }
        public double TotalTaxPrice { get; set; }
        public double TotalShippingPrice { get; set; }
        public double TotalPrice { get; set; }
        public long TotalOrders { get; set; }
    }
}
