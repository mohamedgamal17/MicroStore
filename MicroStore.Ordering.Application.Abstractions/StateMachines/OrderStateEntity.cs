#nullable disable
using MassTransit;

namespace MicroStore.Ordering.Application.Abstractions.StateMachines
{
    public class OrderStateEntity : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string OrderNumber { get; set; }
        public string CurrentState { get; set; }
        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public string UserId { get; set; }
        public string PaymentId { get; set; }
        public string ShipmentId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public List<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
        public string CancellationReason { get; set; }
        public DateTime? CancellationDate { get; set; }
    }
}
