#pragma warning disable CS8618
using Volo.Abp.Domain.Entities;
namespace MicroStore.Geographic.Application.Dtos
{
    public class StateProvinceDto : Entity<string>
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public string CountryId { get; set; }

    }
}
