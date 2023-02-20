#pragma warning disable CS8618
using Volo.Abp.Application.Dtos;
namespace MicroStore.Catalog.Application.Dtos
{
    public class CategoryDto : EntityDto<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
