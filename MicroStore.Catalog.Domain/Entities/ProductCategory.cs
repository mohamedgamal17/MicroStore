using Volo.Abp.Domain.Entities;
namespace MicroStore.Catalog.Domain.Entities
{
    public class ProductCategory : Entity<Guid>
    {
        public Category Category { get;  set; }
        public Guid CategoryId { get; set; }
        public bool IsFeaturedProduct { get; private set; }


        public ProductCategory(Guid categoryId)
            : base(Guid.NewGuid())
        {
            CategoryId = categoryId;
        }

        public ProductCategory()
        {

        }
        public void SetFeaturedProduct(bool isFeatured) => IsFeaturedProduct = isFeatured;


    }
}
