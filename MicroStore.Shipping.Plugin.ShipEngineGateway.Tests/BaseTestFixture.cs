using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Settings;
using MicroStore.Shipping.PluginInMemoryTest;
using MicroStore.Shipping.Domain.ValueObjects;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Consts;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using Microsoft.Extensions.Configuration;

namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Tests
{
    public class BaseTestFixture : PluginTestFixture<ShipEngineGatewayTestModule>
    {

        public ISettingsRepository SettingsRepository { get; set; }

        [OneTimeSetUp]
        public async Task SetupBeforeAnyTest()
        {

            var config = ServiceProvider.GetRequiredService<IConfiguration>();

            var settings = new ShipEngineSettings
            {
                ApiKey = config.GetValue<string>("ApiKey"),
               
            };

            SettingsRepository = ServiceProvider.GetRequiredService<ISettingsRepository>();

            await SettingsRepository.TryToUpdateSettrings(settings);
        }



        public Task<Shipment> CreateShipmentWithInitialStatus()
        {
            MicroStore.Shipping.Domain.ValueObjects.Address address = new AddressBuilder()
                .WithName("john doe")
                .WithPhone("555-555-5555")
                .WithCountryCode("US")
                .WithState("TX")
                .WithCity("Austin")
                .WithAddressLine("525 S Winchester Blvd", "525 S Winchester Blvd")
                .WithPostalCode("75462")
                .WithZip("73301")
                .Build();

            Shipment shipment = new Shipment(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), address)
            {
                Items = new List<MicroStore.Shipping.Domain.Entities.ShipmentItem>
                {
                    new MicroStore.Shipping.Domain.Entities.ShipmentItem
                    {
                        Name= Guid.NewGuid().ToString(),
                        Sku= Guid.NewGuid().ToString(),
                        ProductId= Guid.NewGuid().ToString(),
                        Thumbnail= Guid.NewGuid().ToString(),
                        Dimension = Dimension.FromInch(6,12,24),
                        Weight = MicroStore.Shipping.Domain.ValueObjects.Weight.FromGram(40),
                        Quantity = 4,
                        UnitPrice = 50

                    }
                },

                Status = ShipmentStatus.Created,
            };

            return InsertAsync(shipment);
        }

        public async Task<Shipment> CreateShipmentWithFullfillStatus()
        {
            var settings = await SettingsRepository.TryToGetSettings<ShipEngineSettings>(ShipEngineConst.SystemName) ??
                new ShipEngineSettings();

            var fakeShipment = await CreateShipmentWithInitialStatus();

            var shipeEngineProvider = ServiceProvider.GetRequiredService<ShipEngineSystemProvider>();

            FullfillModel model = new FullfillModel
            {
                Package = new PackageModel
                {
                    Dimension = new DimensionModel
                    {
                        Lenght = 24,
                        Width = 12,
                        Height = 6,
                        Unit = "inch"
                    },

                    Weight = new WeightModel
                    {
                        Value = 40,
                        Unit = "g"
                    }
                },
                AddressFrom = new AddressModel
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

            await shipeEngineProvider.Fullfill(fakeShipment.Id, model, CancellationToken.None);

            return await SingleAsync<Shipment>(x => x.Id == fakeShipment.Id);
        }

        public async Task<Shipment> CreateShipmentWithShippingStatus()
        {
            var shipeEngineProvider = ServiceProvider.GetRequiredService<ShipEngineSystemProvider>();

            var fakeShipment = await CreateShipmentWithFullfillStatus();

            var rates = await shipeEngineProvider.RetriveShipmentRates(fakeShipment.ShipmentExternalId!);

            BuyShipmentLabelModel model = new BuyShipmentLabelModel
            {
                ShipmentRateId = rates.GetEnvelopeResult<List<ShipmentRateDto>>().Result.First().RateId
            };

            await shipeEngineProvider.BuyShipmentLabel(fakeShipment.ShipmentExternalId!, model, CancellationToken.None);

            return await SingleAsync<Shipment>(x => x.Id == fakeShipment.Id);
        }

    }
}
