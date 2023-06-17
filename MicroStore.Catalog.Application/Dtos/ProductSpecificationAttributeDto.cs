using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Application.Dtos
{
    public class ProductSpecificationAttributeDto : Entity<string>
    {
        public SpecificationAttributeListDto Attribute { get; set; }
        public SpecificationAttributeOptionDto Option { get; set; }


    }
}
