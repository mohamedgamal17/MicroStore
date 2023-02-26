#pragma warning disable CS8618
using Volo.Abp.Domain.Entities;

namespace MicroStore.Geographic.Application.Domain
{
    public class Country : Entity<string>
    {
        public Country()
        {
            Id = Guid.NewGuid().ToString();

        //    StateProvinces = new List<StateProvince>();
        }
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
        public int NumericIsoCode { get; set; }
        public List<StateProvince> StateProvinces { get; set; } = new List<StateProvince>();
    }
}
