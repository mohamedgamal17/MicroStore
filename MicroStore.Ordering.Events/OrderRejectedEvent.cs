namespace MicroStore.Ordering.Events
{
    public class OrderRejectedEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string RejectReason { get; set; }
        public DateTime RejectedDate { get; set; }

        public string? RejectedBy { get; set; }
    }
}
