using Volo.Abp.Domain.Entities;
namespace MicroStore.Catalog.Domain.Entities
{
    public class ProductImage : Entity<Guid>
    {
        public string ImagePath { get; set; }
        public int DisplayOrder { get; set; }


        public ProductImage()
        {
            Id = Guid.NewGuid();
        }

    }
}
