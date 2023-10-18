using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Operations.Etos
{
    public class ProductImageEto : EntityDto<string>
    {
        public string Image { get; set; }
        public int DisplayOrder { get; set; }
    }
}
