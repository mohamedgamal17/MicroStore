using Volo.Abp.Domain.Entities;

namespace MicroStore.Shipping.Domain.Entities
{
    public class ShippingSystem : Entity<Guid>
    {
        public string  Name { get; set; }
        public string DisplayName { get; set; }
        public string  Image { get; set; }
        public bool IsEnabled { get; set; }
    }
}
