using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
namespace MicroStore.TestBase
{
    public class MassTransitTestBase<TStartupModule> : ApplicationTestBase<TStartupModule>
    where TStartupModule : AbpModule
    {

        public ITestHarness TestHarness { get; }

        public MassTransitTestBase()
        {
            TestHarness = ServiceProvider.GetRequiredService<ITestHarness>();
        }

        public async Task StartMassTransit()
        {
            await TestHarness.Start();
        }


        public async Task StopMassTransit()
        {
            await TestHarness.Stop();
        }
    }
}
