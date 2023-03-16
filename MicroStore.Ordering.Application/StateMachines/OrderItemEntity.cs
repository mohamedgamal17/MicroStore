#nullable disable
using MicroStore;

namespace MicroStore.Ordering.Application.StateMachines
{
    public class OrderItemEntity
    {
        public Guid Id { get; set; }
        public string ExternalProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string? Thumbnail { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
