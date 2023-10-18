using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Operations.Etos
{
    public class CategoryEto : FullAuditedEntityDto<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
