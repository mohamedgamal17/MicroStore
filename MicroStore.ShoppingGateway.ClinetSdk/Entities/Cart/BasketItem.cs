namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart
{
    public class BasketItem : BaseEntity<Guid>
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
