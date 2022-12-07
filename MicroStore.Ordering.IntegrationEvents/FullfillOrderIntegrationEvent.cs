using System.Reflection.Metadata.Ecma335;

namespace MicroStore.Ordering.IntegrationEvents
{
    public class FullfillOrderIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string ShipmentId { get; set; }
        public string  ShipmentSystem { get; set; }

    }
}
