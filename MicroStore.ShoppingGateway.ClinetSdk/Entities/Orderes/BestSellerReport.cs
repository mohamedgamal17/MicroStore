namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes
{
    public class BestSellerReport : BaseEntity<string>
    {
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public double Amount { get; set; }
        public double Quantity { get; set; }
    }
}
