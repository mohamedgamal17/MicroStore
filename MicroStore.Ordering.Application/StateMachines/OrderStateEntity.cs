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
        public decimal ShippingCost { get; set; }
        public decimal TaxCost { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalPrice { get; set; }
        public string UserId { get; set; }
        public string PaymentId { get; set; }
        public string ShipmentId { get; set; }
        public string ShipmentSystem { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public List<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
        public string CancellationReason { get; set; }
        public DateTime? CancellationDate { get; set; }
    }
}
