


using Volo.Abp.Domain.Entities;


namespace MicroStore.Catalog.Domain.Entities
{
    public class ProductImage : Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public string ImagePath { get; set; }
        public bool IsDefault { get; set; }


        public ProductImage(Guid productId, string imagePath, bool isDefault)
           : this(Guid.NewGuid(), productId, imagePath, isDefault)
        {

        }

        public ProductImage(Guid id, Guid productId, string imagePath, bool isDefault)
        {
            Id = id;
            ProductId = productId;
            ImagePath = imagePath;
            IsDefault = isDefault;
        }
    }
}
