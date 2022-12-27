using Volo.Abp.Domain.Entities;

namespace MicroStore.Payment.Domain
{
    public class PaymentSystem : Entity<Guid>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public bool IsEnabled { get; set; }
    }
}
