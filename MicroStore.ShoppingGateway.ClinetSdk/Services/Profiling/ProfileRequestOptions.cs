using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling
{
    
    public class ProfileRequestOptions
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public string? Avatar { get; set; }
        public List<AddressRequestOptions> Addresses { get; set; }
    }


    public class AddressRequestOptions
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string CountryCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Zip { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
    }


    public enum Gender
    {
        Male,
        Female
    }
}
