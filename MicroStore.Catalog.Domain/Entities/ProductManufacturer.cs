using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Domain.Entities
{
    public class ProductManufacturer : Entity<string>
    {
        public string  ProductId { get; set; }
        public string  ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public ProductManufacturer()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
