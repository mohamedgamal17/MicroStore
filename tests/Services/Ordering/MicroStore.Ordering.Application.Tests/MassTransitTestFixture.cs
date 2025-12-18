using MicroStore.TestBase;

namespace MicroStore.Ordering.Application.Tests
{
    [TestFixture]
    public abstract class MassTransitTestFixture : MassTransitTestBase<StartupModule>
    {


        [OneTimeSetUp]
        public async Task SetupBeforeAllTests()
        {
            await StartMassTransit();
        }


        [OneTimeTearDown]
        public async Task OnAllTestsTearDown()
        {
            await StopMassTransit();
        }
    }
}
