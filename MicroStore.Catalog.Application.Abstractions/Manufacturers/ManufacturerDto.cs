using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Abstractions.Manufacturers
{
    public class ManufacturerDto : FullAuditedEntityDto<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
