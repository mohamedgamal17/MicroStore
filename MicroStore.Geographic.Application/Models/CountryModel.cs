#pragma warning disable CS8618
namespace MicroStore.Geographic.Application.Models
{
    public class CountryModel
    {
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
        public int NumericIsoCode { get; set; }
    }
}
