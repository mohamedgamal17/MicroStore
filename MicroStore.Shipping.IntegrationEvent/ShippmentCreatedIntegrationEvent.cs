namespace MicroStore.Shipping.IntegrationEvent
{
    public class ShippmentCreatedIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string ShippmentId { get; set; }
    }
}
