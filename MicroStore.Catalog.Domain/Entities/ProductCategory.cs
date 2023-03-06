#pragma warning disable CS8618
using Volo.Abp.Domain.Entities;
namespace MicroStore.Catalog.Domain.Entities
{
    public class ProductCategory : Entity<string>
    {
        public Category Category { get;  set; }
        public string CategoryId { get; set; }

        public ProductCategory()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
