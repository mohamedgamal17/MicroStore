using MicroStore.Catalog.Application.Abstractions.Manufacturers;
using Volo.Abp.Application.Dtos;
namespace MicroStore.Catalog.Application.Abstractions.Products
{
    public class ProductManufacturerDto : EntityDto<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
