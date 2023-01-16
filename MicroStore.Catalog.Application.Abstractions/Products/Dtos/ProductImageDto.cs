using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Abstractions.Products.Dtos
{
    public class ProductImageDto :EntityDto<Guid>
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public int DisplayOrder { get; set; }
    }
}
