


using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Domain.Entities
{
    public class ProductCategory : Entity<Guid>
    {
        public Category Category { get; private set; }
        public Guid CategoryId { get; set; }
        public bool IsFeaturedProduct { get; private set; }


        public ProductCategory(Guid categoryId)
            : base(Guid.NewGuid())
        {
            CategoryId = categoryId;
        }

        protected ProductCategory() // Require For EFCore
        {

        }
        public void SetFeaturedProduct(bool isFeatured) => IsFeaturedProduct = isFeatured;


    }
}
