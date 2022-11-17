namespace MicroStore.Ordering.Events.Models
{
    public class OrderResponse
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public Guid BillingAddressId { get; set; }
        public Guid ShippingAddressId { get; set; }
        public string PaymentId { get; set; }
        public string ShippmentId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
        public string CancelledBy { get; set; }
        public string RejectedBy { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? PaymentAcceptedDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string FaultReason { get; set; }
        public string CancellationReason { get; set; }
        public DateTime? FaultDate { get; set; }
        public DateTime? CancellationDate { get; set; }
    }
}
