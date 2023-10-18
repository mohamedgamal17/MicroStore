#pragma warning disable CS8618
using Volo.Abp.Domain.Entities.Auditing;
namespace MicroStore.Geographic.Application.Domain
{
    public class Country : FullAuditedAggregateRoot<string>
    {
        public Country()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
        public int NumericIsoCode { get; set; }
        public List<StateProvince> StateProvinces { get; set; } = new List<StateProvince>();
    }
}
