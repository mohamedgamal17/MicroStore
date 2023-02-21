#pragma warning disable CS8618

namespace MicroStore.Inventory.Application.Models
{
    public class AllocateOrderStockModel
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public string PaymentId { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddres { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderItemModel> Items { get; set; }
    }
}
