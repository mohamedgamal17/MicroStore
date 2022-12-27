namespace MicroStore.Shipping.IntegrationEvents
{
    public class ShipmentCompletedIntegrationEvent
    {
        public string ShipmentId { get; set; }
        public string OrderId { get; set; }
        public string  UserId { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
