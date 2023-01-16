using MicroStore.ShoppingGateway.ClinetSdk.Common;
using System.Runtime.Serialization;

namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes
{
    public class Order  : BaseEntity<Guid>
    {
        public string OrderNumber { get; set; }
        public OrderState CurrentState { get; set; }
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
        public List<OrderItem> Items { get; set; } 
        public string CancellationReason { get; set; }
        public DateTime? CancellationDate { get; set; }

    }

    public enum OrderState
    {
        [EnumMember(Value = "Submited")]
        Submited,

        [EnumMember(Value = "Accepted")]
        Accepted,

        [EnumMember(Value = "Approved")]
        Approved,

        [EnumMember(Value = "Fullfilled")]
        Fullfilled,

        [EnumMember(Value = "Completed")]
        Completed,

        [EnumMember(Value = "Cancelled")]
        Cancelled
    }
}
