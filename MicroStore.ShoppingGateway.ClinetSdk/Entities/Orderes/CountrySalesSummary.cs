namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes
{
    public class CountrySalesSummary
    {
        public string CountryCode { get; set; }
        public double TotalTaxPrice { get; set; }
        public double TotalShippingPrice { get; set; }
        public double TotalPrice { get; set; }
        public long TotalOrders { get; set; }
    }
}
