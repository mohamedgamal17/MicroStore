using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Domain.Entities
{
    public class Manufacturer : BasicAggregateRoot<string>
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public Manufacturer()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
