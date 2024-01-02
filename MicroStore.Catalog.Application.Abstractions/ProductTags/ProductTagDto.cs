using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Abstractions.ProductTags
{
    public class ProductTagDto : FullAuditedEntityDto<string>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
