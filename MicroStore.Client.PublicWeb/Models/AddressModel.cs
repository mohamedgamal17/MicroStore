using System.ComponentModel;

namespace MicroStore.Client.PublicWeb.Models
{
    public class AddressModel
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Country Code")]
        public string Country { get; set; }

        [DisplayName("State Province")]
        public string StateProvince { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("Address Line 1")]
        public string AddressLine1 { get; set; }

        [DisplayName("Address Line 1")]
        public string? AddressLine2 { get; set; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
