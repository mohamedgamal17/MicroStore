using FluentAssertions;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Tests.Fakes;
namespace MicroStore.Shipping.Application.Tests.Commands
{
    public class UpdateShippingSettingsCommandHandlerTests : BaseTestFixture
    {

        [Test]
        public async Task Should_update_shipping_settings()
        {
            var command = new UpdateShippingSettingsCommand
            {
                DefaultShippingSystem = FakeConst.NotActiveSystem,
                Location = new AddressModel
                {
                    CountryCode = "US",
                    State = "CA",
                    City = "San Jose",
                    AddressLine1 = "525 S Winchester Blvd",
                    AddressLine2 = "525 S Winchester Blvd",
                    Name = "Jane Doe",
                    Phone = "555-555-5555",
                    PostalCode = "95128",
                    Zip = "90241"
                }
            };

            var result = await Send(command);

            var settings = await TryToGetSettings();

            settings.DefaultShippingSystem.Should().Be(command.DefaultShippingSystem);

            settings.Location.Should().NotBeNull();

            settings.Location!.Name.Should().Be(command.Location.Name);
            settings.Location!.Phone.Should().Be(command.Location.Phone);
            settings.Location!.CountryCode.Should().Be(command.Location.CountryCode);
            settings.Location!.State.Should().Be(command.Location.State);
            settings.Location!.City.Should().Be(command.Location.City);
            settings.Location!.AddressLine1.Should().Be(command.Location.AddressLine1);
            settings.Location!.AddressLine2.Should().Be(command.Location.AddressLine2);
            settings.Location!.Zip.Should().Be(command.Location.Zip);
            settings.Location!.PostalCode.Should().Be(command.Location.PostalCode);
        }
    }
}
