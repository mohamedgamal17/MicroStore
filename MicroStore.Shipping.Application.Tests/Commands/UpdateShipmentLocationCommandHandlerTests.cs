using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;

namespace MicroStore.Shipping.Application.Tests.Commands
{
    public class UpdateShipmentLocationCommandHandlerTests : BaseTestFixture
    {
        [Test]
        public  async Task Should_update_shipping_location_settings()
        {
            var settingsRepository = ServiceProvider.GetRequiredService<ISettingsRepository>();

            var command = PrepareCommand();

            await Send(command);

            var settings = await settingsRepository.TryToGetSettings<ShippingSettings>(SettingsConst.ProviderKey);

            settings.Location.Name.Should().Be(command.Name);
            settings.Location.CountryCode.Should().Be(command.CountryCode);
            settings.Location.City.Should().Be(command.City);
            settings.Location.PostalCode.Should().Be(command.PostalCode);
            settings.Location.Zip.Should().Be(command.Zip);
            settings.Location.AddressLine1.Should().Be(command.AddressLine1);
            settings.Location.AddressLine2.Should().Be(command.AddressLine2);
        }




        private UpdateShippingLocationCommand PrepareCommand()
        {
            return new UpdateShippingLocationCommand
            {
                Name = Guid.NewGuid().ToString(),
                CountryCode = "US",
                State = "AL",
                City = "Alaska",
                PostalCode = "5445",
                Zip = "5412",
                AddressLine1 = "NAN",
                AddressLine2 = "NAN"
            };
        }
    }
}
