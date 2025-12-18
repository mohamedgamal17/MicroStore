#nullable disable
using MassTransit;

namespace MicroStore.Ordering.Application.Domain
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
        public string PaymentId { get; set; } = string.Empty;
        public string ShipmentId { get; set; } = string.Empty;
        public DateTime SubmissionDate { get; set; }
        public DateTime ShippedDate { get; set; } = DateTime.MinValue;
        public List<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
        public string CancellationReason { get; set; } = string.Empty;
        public DateTime CancellationDate { get; set; } = DateTime.MinValue;
    }
}
