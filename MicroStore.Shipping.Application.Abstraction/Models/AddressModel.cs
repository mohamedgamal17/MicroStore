using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class AddressModel
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
                      .WithZip(Zip)
                      .Build();
        }


        public AddressDto AsAddressDto()
        {
            return new AddressDto
            {
                Name = Name,
                Phone = Phone,
                CountryCode = CountryCode,
                City = City,
                State = State,
                PostalCode = PostalCode,
                Zip = Zip,
                AddressLine1 = AddressLine1,
                AddressLine2 = AddressLine2,

            };
        }

    }
}
