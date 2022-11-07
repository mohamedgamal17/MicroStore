namespace MicroStore.Shipping.IntegrationEvent
{
    public class ShippmentFaildIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string ShippmentId { get; set; }
        public string FaultReason { get; set; }
        public DateTime FaultDate { get; set; }
    }
}
