using FluentAssertions;
using MicroStore.Profiling.Application.Domain;
using MicroStore.Profiling.Application.Models;

namespace MicroStore.Profiling.Application.Tests.Extensions
{
    public static class AssertionExtensions
    {
        public static void AssertProfile(this Profile profile , ProfileModel model)
        {
            profile.FirstName.Should().Be(model.FirstName);
            profile.LastName.Should().Be(model.LastName);
            profile.Phone.AssertPhone(model.Phone);
            profile.BirthDate.Should().Be(model.BirthDate);
            profile.Gender.Should().Be(Enum.Parse<Gender>(model.Gender.ToLower(), true));

            if(profile.Addresses != null && model.Addresses != null)
            {
                foreach (var tuple in Enumerable.Zip(profile.Addresses, model.Addresses))
                {
                    tuple.First.AssertAddress(tuple.Second);
                }
            }

        }

        public static void AssertPhone(this Phone phone, PhoneModel model)
        {
            var expected = Phone.Create(model.Number, model.CountryCode);
           
            phone.Should().Be(expected);
        }

        public static void AssertAddress(this Address address , AddressModel model)
        {
            address.Name.Should().Be(model.Name);
            address.CountryCode.Should().Be(model.CountryCode);
            address.City.Should().Be(model.City);
            address.State.Should().Be(model.State);
            address.AddressLine1.Should().Be(model.AddressLine1);
            address.AddressLine2.Should().Be(model.AddressLine2);
            address.PostalCode.Should().Be(model.PostalCode);
            address.Zip.Should().Be(model.Zip);
        }
    }
}
