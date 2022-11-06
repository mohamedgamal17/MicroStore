using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.TestBase;
using Respawn;
using Respawn.Graph;
namespace MicroStore.Catalog.Application.Tests
{
    [TestFixture]
    public abstract class BaseTestFixture : MassTransitTestBase<StartupModule>
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
        }






        [TearDown]
        protected async Task SetupAfterRunAnyTest()
        {
            var configuration = ServiceProvider.GetRequiredService<IConfiguration>();

            await Respawner.ResetAsync(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
