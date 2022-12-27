using MicroStore.Shipping.Domain.ValueObjects;
using Volo.Abp.Domain.Entities;
namespace MicroStore.Shipping.Domain.Entities
{
    public class ShipmentItem : Entity<Guid>
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ProductId { get; set; }
        public string Thumbnail { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Weight Weight { get; set; }
        public Dimension Dimension { get; set; }

    }
}
