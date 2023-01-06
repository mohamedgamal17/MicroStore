using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Ordering.Infrastructure.EntityFramework;
using MicroStore.TestBase;
using System.Linq.Expressions;
namespace MicroStore.Ordering.Application.Tests
{
    [TestFixture]
    public abstract class StateMachineTestFixture<TStateMachine, TInstance> : StateMachineTestBase<StartupModule,TStateMachine,TInstance>
           where TStateMachine : class, SagaStateMachine<TInstance>
           where TInstance : class, SagaStateMachineInstance
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


        public Task<TInstance> Find(Expression<Func<TInstance,bool>> expression)
        {
            using var scope = ServiceProvider.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

            return db.Set<TInstance>().SingleAsync(expression);
        }
    }
}
