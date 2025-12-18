namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Cart
{
    public class BaskeItemRequestOptions
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class BasketRequestOptions
    {
        public List<BaskeItemRequestOptions> Items { get; set; }

    }

    public class BasketRemoveItemRequestOptions
    {
        public string ProductId { get; set; }

        public int? Quantity { get; set; }
    }

    public class BasketMigrateRequestOptions
    {
        public string FromUserId { get; set; }

        public string ToUserId { get; set; }
    }
}
