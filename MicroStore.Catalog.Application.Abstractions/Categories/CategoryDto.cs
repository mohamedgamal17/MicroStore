#pragma warning disable CS8618
using Volo.Abp.Application.Dtos;
namespace MicroStore.Catalog.Application.Abstractions.Categories
{
    public class CategoryDto : FullAuditedEntityDto<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
