namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing
{
    public class PaymentSystem : BaseEntity<string>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public bool IsEnabled { get; set; }
    }
}
