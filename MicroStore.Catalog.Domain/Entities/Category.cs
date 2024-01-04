#pragma warning disable CS8618
using Volo.Abp.Domain.Entities.Auditing;
namespace MicroStore.Catalog.Domain.Entities
{
    public class Category : AuditedAggregateRoot<string>
    {

        public string Name { get;  set; }
        public string Description { get;  set; } = string.Empty;
        public List<Product> Products { get; set; }
        public Category()
        {
            Id = Guid.NewGuid().ToString();
        }
   
    }
}
