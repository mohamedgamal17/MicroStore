#nullable disable
using MassTransit;

namespace MicroStore.Ordering.Application.StateMachines
{
    public class OrderStateEntity : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string OrderNumber { get; set; }
        public string CurrentState { get; set; }
        public Guid ShippingAddressId { get; set; }
        public Guid BillingAddressId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string UserId { get; set; }
        public string PaymentId { get; set; }
        public string ShippmentId { get; set; }
        public string CancelledBy { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? PaymentAcceptedDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public List<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
        public string FaultReason { get; set; }
        public string CancellationReason { get; set; }
        public DateTime? FaultDate { get; set; }
        public DateTime? CancellationDate { get; set; }
    }
}
