namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart
{
    [Serializable]
    public class Basket : BaseEntity<string>
    {
        public string UserName { get; set; }

        public List<BasketItem> Items { get; set; }
    }
}
