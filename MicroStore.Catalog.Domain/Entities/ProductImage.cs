#pragma warning disable CS8618
using Volo.Abp.Domain.Entities;
namespace MicroStore.Catalog.Domain.Entities
{
    public class ProductImage : Entity<string>
    {
        public string ImagePath { get; set; }
        public int DisplayOrder { get; set; }
        public string ProductId { get; set; }
        public ProductImage()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
