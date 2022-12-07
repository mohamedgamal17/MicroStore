using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Ordering.Infrastructure.EntityFramework;
using MicroStore.TestBase;
using Respawn;
using Respawn.Graph;
using System.Linq.Expressions;

namespace MicroStore.Ordering.Application.Tests
{
    [TestFixture]
    public abstract class StateMachineTestFixture<TStateMachine, TInstance> : StateMachineTestBase<StartupModule,TStateMachine,TInstance>
           where TStateMachine : class, SagaStateMachine<TInstance>
           where TInstance : class, SagaStateMachineInstance
    {

        public Respawner Respawner { get; private set; }

        [OneTimeSetUp]
        public async Task SetupBeforeAllTests()
        {
            var config = ServiceProvider.GetRequiredService<IConfiguration>();

            Respawner = await Respawner.CreateAsync(config.GetConnectionString("DefaultConnection"), new RespawnerOptions
            {
                TablesToIgnore = new Table[]
                {
                    "__EFMigrationsHistory"
                } 
            });

            await SetupBeforeAnyTest();
        }

        [OneTimeTearDown]
        public async Task OnAllTestsTearDown()
        {
            await OnTearDown();
        }


        public Task<TInstance> Find(Expression<Func<TInstance,bool>> expression)
        {
            using var scope = ServiceProvider.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

            return db.Set<TInstance>().SingleAsync(expression);
        }
    }
}
