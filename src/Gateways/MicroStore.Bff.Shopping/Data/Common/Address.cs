namespace MicroStore.Bff.Shopping.Data.Common
{
    public class Address
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public AddressCountry Country { get; set; }
        public AddressStateProvince State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Zip { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
    }
}
