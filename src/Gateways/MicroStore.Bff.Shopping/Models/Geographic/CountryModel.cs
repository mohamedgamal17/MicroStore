namespace MicroStore.Bff.Shopping.Models.Geographic
{
    public class CountryModel
    {
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
        public int NumericIsoCode { get; set; }

        public CountryModel()
        {
            Name = string.Empty;
            TwoLetterIsoCode = string.Empty;
            ThreeLetterIsoCode = string.Empty;
            NumericIsoCode = 0;
        }
    }
}
