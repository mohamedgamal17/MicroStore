namespace MicroStore.Shipping.IntegrationEvent
{
    public class CreateShippmentIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
    }
}