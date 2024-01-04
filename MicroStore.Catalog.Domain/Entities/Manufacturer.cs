using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace MicroStore.Catalog.Domain.Entities
{
    public class Manufacturer : AuditedAggregateRoot<string>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<Product> Products { get; set; }

        public Manufacturer()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
