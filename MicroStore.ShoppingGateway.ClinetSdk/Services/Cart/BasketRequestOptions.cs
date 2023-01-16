namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Cart
{
    public class BasketAddItemRequestOptions
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class BasketRemoveItemRequestOptions
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
    }

    public class BasketMigrateRequestOptions
    {
        public string FromUserId { get; set; }

        public string ToUserId { get; set; }
    }
}
