#pragma warning disable CS8618
namespace MicroStore.Ordering.Application.Models
{
    public class OrderModel
    {
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddress { get; set; }
        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }

    public class CreateOrderModel : OrderModel
    {
        public string UserId { get; set; }
    }

    public class FullfillOrderModel
    {
        public string ShipmentId { get; set; }
    }

    public class CompleteOrderModel
    {
        public DateTime ShippedAt { get; set; }
    }

    public class CancelOrderModel
    {
        public string Reason { get; set; }

    }


}
