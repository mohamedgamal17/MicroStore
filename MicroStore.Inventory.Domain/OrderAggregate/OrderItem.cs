#pragma warning disable CS8618
using Volo.Abp.Domain.Entities;
namespace MicroStore.Inventory.Domain.OrderAggregate
{
    public class OrderItem : Entity<string>
    {
        public OrderItem(string id)
        {
            Id = id;
        }
        public OrderItem() 
        {
            Id = Guid.NewGuid().ToString(); 
        }

        public string ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
