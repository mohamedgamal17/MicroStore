using MicroStore.Ordering.Application.Models;

namespace MicroStore.Ordering.Api.Models
{
    public class OrderModel
    {
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddress { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
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


    public class CancelOrderModel
    {
        public string Reason { get; set; }

        public DateTime CancellationDate { get; set; }
    }

}
