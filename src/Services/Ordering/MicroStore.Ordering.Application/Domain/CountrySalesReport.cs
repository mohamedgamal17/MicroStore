namespace MicroStore.Ordering.Application.Domain
{
    public class CountrySalesReport
    {
        public string CountryCode { get; set; }
        public double TotalTaxPrice { get; set; }
        public double TotalShippingPrice { get; set; }
        public double TotalPrice { get; set; }
        public long TotalOrders { get; set; }
        public DateTime Date { get; set; }

    }
}
