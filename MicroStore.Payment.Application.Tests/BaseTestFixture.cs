using MicroStore.TestBase;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.Linq.Expressions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Payment.Application.Tests
{
    [TestFixture]
    public class BaseTestFixture : MassTransitTestBase<PaymentApplicationTestModule>
    {
        [OneTimeSetUp]
        protected async Task SetupBeforeAllTests()
        {
            await StartMassTransit();
        }

        [OneTimeTearDown]
        protected async Task SetupAfterRunAllTest()
        {
            await StopMassTransit();
        }


    }
}
