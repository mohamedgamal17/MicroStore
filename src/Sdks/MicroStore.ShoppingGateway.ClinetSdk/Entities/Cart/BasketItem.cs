namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart
{
    public class BasketItem : BaseEntity<string>
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
