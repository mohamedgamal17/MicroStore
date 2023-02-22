using FluentAssertions;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Settings;
using MicroStore.Shipping.Application.Tests.Fakes;

namespace MicroStore.Shipping.Application.Tests.Settings
{
    public class ApplicationSettingsServiceTests : BaseTestFixture
    {
        private readonly IApplicationSettingsService _applicationSettingsService;

        public ApplicationSettingsServiceTests()
        {
            _applicationSettingsService =  GetRequiredService<IApplicationSettingsService>();
        }

        [Test]
        public async Task Should_update_shipping_settings()
        {
            var model = new UpdateShippingSettingsModel
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

            await _applicationSettingsService.UpdateAsync(model);

            var settings = await TryToGetSettings();

            settings.DefaultShippingSystem.Should().Be(model.DefaultShippingSystem);

            settings.Location.Should().NotBeNull();

            settings.Location!.Name.Should().Be(model.Location.Name);
            settings.Location!.Phone.Should().Be(model.Location.Phone);
            settings.Location!.CountryCode.Should().Be(model.Location.CountryCode);
            settings.Location!.State.Should().Be(model.Location.State);
            settings.Location!.City.Should().Be(model.Location.City);
            settings.Location!.AddressLine1.Should().Be(model.Location.AddressLine1);
            settings.Location!.AddressLine2.Should().Be(model.Location.AddressLine2);
            settings.Location!.Zip.Should().Be(model.Location.Zip);
            settings.Location!.PostalCode.Should().Be(model.Location.PostalCode);
        }

    
    }
}
