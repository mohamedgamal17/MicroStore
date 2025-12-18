namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Geographic
{
    public class StateProvince : BaseEntity<string>
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string CountryId { get; set; }
    }
}
