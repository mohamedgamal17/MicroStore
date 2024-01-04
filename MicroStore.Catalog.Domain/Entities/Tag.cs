using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Domain.Entities
{
    public class Tag : Entity<string>
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();

        public Tag()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
