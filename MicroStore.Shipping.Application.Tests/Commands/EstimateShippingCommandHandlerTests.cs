using FluentAssertions;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Tests.Fakes;
using MicroStore.Shipping.Domain.Entities;

namespace MicroStore.Shipping.Application.Tests.Commands
{
    public class EstimateShippingCommandHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_return_estimated_rates_with_success_result()
        {
            var command = PrepareEstimateShipRateCommand();

            var result = await Send(command);

            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public async Task Should_return_failure_result_when_shipping_settings_is_not_configured()
        {
            var settings = new ShippingSettings();

            await UpdateSettings(settings);

            var command = PrepareEstimateShipRateCommand();

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();
        }

        [Test]
        public async Task Should_return_failure_result_when_location_address_is_not_configured()
        {
            var settings = new ShippingSettings();

            settings.DefaultShippingSystem = FakeConst.ActiveSystem;

            await UpdateSettings(settings);

            var command = PrepareEstimateShipRateCommand();

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();
        }


        private EstimateShipmentRateCommand PrepareEstimateShipRateCommand()
        {
            return new EstimateShipmentRateCommand
            {
                Address = new AddressModel
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
                },
                Items = new List<ShipmentItemEstimationModel>
                {
                    new ShipmentItemEstimationModel
                    {
                        Sku = Guid.NewGuid().ToString(),
                        Name= Guid.NewGuid().ToString(),
                        Quantity = 5,
                        UnitPrice = new MoneyDto
                        {
                            Currency = "usd",
                            Value =50
                        },
                        Weight = new WeightModel
                        {
                            Unit = "g",
                            Value = 50
                        }
                    }
                }
            };
        }
    }
}
