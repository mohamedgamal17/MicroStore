using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.TestBase;
using NUnit.Framework;
using Respawn;
using Respawn.Graph;
using System.Linq.Expressions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests
{
    [TestFixture]
    public abstract class BaseTestFixture : MassTransitTestBase<StartupModule>
    {

        [OneTimeSetUp]
        protected async Task SetupBeforeAllTests()
        {         
            await StartMassTransit();
        }

        [OneTimeTearDown]
        protected async Task SetupAfterRunAnyTest()
        {
            await StopMassTransit();
        }

    }
}
