#pragma warning disable CS8618
using Volo.Abp.Application.Dtos;
namespace MicroStore.Catalog.Application.Dtos
{
    public class ProductImageDto : EntityDto<string>
    {
        public string ProductId { get; set; }

        public string Image { get; set; }

        public int DisplayOrder { get; set; }
    }
}
