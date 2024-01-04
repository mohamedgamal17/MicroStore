#pragma warning disable CS8618
using Volo.Abp.Application.Dtos;
namespace MicroStore.Catalog.Application.Abstractions.Products
{
    public class ProductCategoryDto : EntityDto<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
