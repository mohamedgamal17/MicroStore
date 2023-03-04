namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Geographic
{
    public class Country
    {
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
        public int NumericIsoCode { get; set; }
        public List<StateProvince>? StateProvinces { get; set; }
    }
}
