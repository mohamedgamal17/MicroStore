using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Tests.Fakes;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.TestBase;
using Respawn;
using Respawn.Graph;
namespace MicroStore.Shipping.Application.Tests
{
    public class BaseTestFixture : MassTransitTestBase<ShippingApplicationTestModule>
    {
        public Respawner Respawner { get; set; }

        [OneTimeSetUp]
        protected async Task SetupBeforeAllTests()
        {

            var configuration = ServiceProvider.GetRequiredService<IConfiguration>();

            Respawner = await Respawner.CreateAsync(configuration.GetConnectionString("DefaultConnection"), new RespawnerOptions
            {
                TablesToIgnore = new Table[]
                {
                   "__EFMigrationsHistory"
                }
            });

            var settings = new ShippingSettings
            {
                DefaultShippingSystem = FakeConst.ActiveSystem,
                Location = new AddressSettings
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

            await UpdateSettings(settings);

            await StartMassTransit();
        }

        [OneTimeTearDown]
        protected async Task SetupAfterAllTests()
        {
            await StopMassTransit();
        }

   

        public async Task<ShippingSettings> TryToGetSettings()
        {
            var repository = ServiceProvider.GetRequiredService<ISettingsRepository>();

            return await repository.TryToGetSettings<ShippingSettings>(SettingsConst.ProviderKey) ?? new ShippingSettings();
        }

        public async Task<ShippingSettings> UpdateSettings(ShippingSettings settings)
        {
            var repository = ServiceProvider.GetRequiredService<ISettingsRepository>();

            await repository.TryToUpdateSettrings(settings);

            return await TryToGetSettings();
        }
    }
}
