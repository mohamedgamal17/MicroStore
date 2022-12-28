using MicroStore.Ordering.Events.Models;

namespace MicroStore.Ordering.Api.Models
{
    public class SubmitOrderModel
    {
        public string UserId { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddress { get; set; }  
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }
}
