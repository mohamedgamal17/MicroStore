#pragma warning disable CS8618
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace MicroStore.Payment.Application.Domain
{
    public class PaymentSystem : AuditedEntity<string>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public bool IsEnabled { get; set; }

        public PaymentSystem()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
