#pragma warning disable CS8618
using MicroStore.Inventory.Domain.ValueObjects;
using Volo.Abp.Domain.Entities;
namespace MicroStore.Inventory.Domain.OrderAggregate
{
    public class Order : BasicAggregateRoot<string>
    {
        public Order(string id)
        {
            Id = id;
        }
        public Order()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public string PaymentId { get; set; }
        public Address ShippingAddress { get; set; }
        public Address BillingAddres { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public bool IsCancelled { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
