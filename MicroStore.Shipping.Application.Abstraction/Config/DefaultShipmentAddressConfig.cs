using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Application.Abstraction.Config
{
    public class DefaultShipmentAddressConfig
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

        public Address AsAddress()
        {
            return new AddressBuilder()
                      .WithCountryCode(CountryCode)
                      .WithCity(City)
                      .WithState(State)
                      .WithPostalCode(PostalCode)
                      .WithAddressLine(AddressLine1, AddressLine2)
                      .WithName(Name)
                      .WithPhone(Phone)
                      .Build();
        }
    }
}
