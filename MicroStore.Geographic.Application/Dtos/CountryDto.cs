#pragma warning disable CS8618
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace MicroStore.Geographic.Application.Dtos
{
    public class CountryDto : FullAuditedAggregateRoot<string>
    {
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
        public int NumericIsoCode { get; set; }
        public List<StateProvinceDto> StateProvinces { get; set; }
    }
}
