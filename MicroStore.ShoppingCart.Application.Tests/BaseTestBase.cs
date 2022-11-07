using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.ShoppingCart.Infrastructure.EntityFramework;
using MicroStore.TestBase;
using Respawn;
using Respawn.Graph;

namespace MicroStore.ShoppingCart.Application.Tests
{
    public class BaseTestBase : MassTransitTestBase<ShoppingCartApplicationTestModule>
    {

        public Respawner Respawner { get; set; }


        public BaseTestBase()
        {

        }

        protected Task<Respawner> CreateRespawner()
        {
            var configuration = ServiceProvider.GetRequiredService<IConfiguration>();

            return Respawner.CreateAsync(configuration.GetConnectionString("DefaultConnection"),
                new RespawnerOptions
                {
                    TablesToIgnore = new Table[]
                    {
                        "__EFMigrationsHistory"
                    }
                });
        }

        protected Task ResetDatabase()
        {
            var configuration = ServiceProvider.GetRequiredService<IConfiguration>();

            return Respawner.ResetAsync(configuration.GetConnectionString("DefaultConnection"));
        }

        [OneTimeSetUp]
        protected async Task OnSetup()
        {
            Respawner = await CreateRespawner();

            await StartMassTransit();

        }


        [TearDown]
        protected async Task OnTestTearDown()
        {
            await ResetDatabase();

        }


        [OneTimeTearDown]
        protected async Task OnTearDown()
        {

            await StopMassTransit();
        }
    }
}
