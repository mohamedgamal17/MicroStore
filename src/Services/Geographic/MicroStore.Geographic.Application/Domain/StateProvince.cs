#pragma warning disable CS8618
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace MicroStore.Geographic.Application.Domain
{
    public class StateProvince : FullAuditedAggregateRoot<string>
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public string CountryId { get; set; }
        public StateProvince()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
