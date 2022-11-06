using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MicroStore.TestBase;
using Moq;
using Respawn;
using Respawn.Graph;

namespace MicroStore.Catalog.Application.Tests
{
    [TestFixture]
    public abstract class BaseTestFixture : MassTransitTestBase<CatalogApplicationTestModule>
    {

        public Mock<IPublishEndpoint> MockedPublishEndPoint = new Mock<IPublishEndpoint>();

        public Respawner Respawner { get; set; }


        protected override void AfterAddApplication(IServiceCollection services)
        {

            var descriptor = new ServiceDescriptor(typeof(IPublishEndpoint), MockedPublishEndPoint.Object);

            services.Replace(descriptor);


        }
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

            await StartMassTransit();
        }

        [TearDown]
        protected async Task SetupAfterRunAnyTest()
        {
            var configuration = ServiceProvider.GetRequiredService<IConfiguration>();

            await Respawner.ResetAsync(configuration.GetConnectionString("DefaultConnection"));
        }

        [OneTimeTearDown]
        protected  async Task AfterAllTestsTearDown()
        {
           await  StopMassTransit();
        }

    }
}
