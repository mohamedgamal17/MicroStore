namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Geographic
{
    public class CountryVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
        public int NumericIsoCode { get; set; }
        public List<StateProvinceVM> StateProvinces { get; set; }
    }
}
