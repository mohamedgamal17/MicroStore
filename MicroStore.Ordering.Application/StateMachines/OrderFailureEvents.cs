
namespace MicroStore.Ordering.Application.StateMachines
{
  
    public class OrderPaymentFaildEvent 
    {
        public Guid OrderId { get; set; }
        public string FaultReason { get; set; }
        public DateTime FaultDate { get; set; }

    }
    public class OrderStockRejectedEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string Details { get; set; }
    }

    public class OrderShippmentFailedEvent
    {
        public Guid OrderId { get; set; }
        public string ShippmentId { get; set; }
        public string Reason { get; set; }
        public DateTime FaultDate { get; set; }
    }
}
