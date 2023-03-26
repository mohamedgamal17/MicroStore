using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Common
{
    public class AddressVM
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


    public class AddressCountryVM
    {
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
        public int NumericIsoCode { get; set; }
    }


    public class AddressStateProvinceVM
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string CountryId { get; set; }
    }
}
