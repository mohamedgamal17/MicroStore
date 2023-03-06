#pragma warning disable CS8618
using Volo.Abp.Application.Dtos;
namespace MicroStore.Catalog.Application.Dtos
{
    public class ProductCategoryDto : EntityDto<string>
    {
        public string CateogryId { get; set; }
        public string Name { get; set; }

    }
}
