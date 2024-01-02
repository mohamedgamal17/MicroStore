#pragma warning disable CS8618
using MicroStore.Catalog.Application.Abstractions.Categories;
using Volo.Abp.Application.Dtos;
namespace MicroStore.Catalog.Application.Abstractions.Products
{
    public class ProductCategoryDto : EntityDto<string>
    {
        public string ProductId { get; set; }
        public string CategoryId { get; set; }
        public CategoryDto Category { get; set; }
    }
}
