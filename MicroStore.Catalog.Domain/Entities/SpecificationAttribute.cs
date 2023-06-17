using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Domain.Entities
{
    public class SpecificationAttribute : Entity<string>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<SpecificationAttributeOption> Options { get; set; } = new List<SpecificationAttributeOption>();
        public SpecificationAttribute()
        {
            Id = Guid.NewGuid().ToString();
        }
   
    }

    public class SpecificationAttributeOption : Entity<string>
    {
        public string Name { get; set; }

        public SpecificationAttributeOption()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
