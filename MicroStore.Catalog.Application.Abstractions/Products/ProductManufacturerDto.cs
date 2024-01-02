using MicroStore.Catalog.Application.Abstractions.Manufacturers;
using Volo.Abp.Application.Dtos;
namespace MicroStore.Catalog.Application.Abstractions.Products
{
    public class ProductManufacturerDto : EntityDto<string>
    {
        public string ManufacturerId { get; set; }
        public string ProductId { get; set; }
        public ManufacturerDto Manufacturer { get; set; }
    }
}
