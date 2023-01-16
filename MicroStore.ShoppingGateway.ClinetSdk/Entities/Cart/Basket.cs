namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart
{
    [Serializable]
    public class Basket : BaseEntity<Guid>
    {
        public string UserId { get; set; }

        public List<BasketItem> Items { get; set; }
    }
}
