using Volo.Abp.Domain.Entities;

namespace MicroStore.Inventory.Domain.OrderAggregate
{
    public class OrderItem : Entity<Guid>
    {
        public OrderItem() :base(Guid.NewGuid())
        {

        }
        public string ExternalItemId { get; set; }
        public string ExternalProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
