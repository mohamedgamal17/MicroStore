namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic
{
    public class CountryRequestOptions
    {
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
        public int NumericIsoCode { get; set; }
    }

    public class CountryListRequestOptions
    {
        public string  Name{ get; set; }
    }
}
