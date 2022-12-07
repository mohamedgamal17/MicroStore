using MassTransit.Testing;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.TestBase.Fakes;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Testing;
using Volo.Abp;
using MicroStore.TestBase.Extensions;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.TestBase
{
    public abstract class StateMachineTestBase<TStartupModule, TStateMachine, TInstance> : AbpIntegratedTest<TStartupModule>
         where TStartupModule : AbpModule
         where TStateMachine : class, SagaStateMachine<TInstance>
         where TInstance : class, SagaStateMachineInstance
    {

        protected ITestHarness TestHarness { get; private set; }
        protected TStateMachine Machine { get; private set; }
        protected ISagaRepository<TInstance> Repository { get; private set; }
        protected ISagaStateMachineTestHarness<TStateMachine, TInstance> SagaHarness { get; private set; }



        public StateMachineTestBase()
        {
            TestHarness = ServiceProvider.GetRequiredService<ITestHarness>();
            Machine = ServiceProvider.GetRequiredService<TStateMachine>();
            Repository = ServiceProvider.GetRequiredService<ISagaRepository<TInstance>>();
            SagaHarness = TestHarness.GetSagaStateMachineHarness<TStateMachine, TInstance>();
        }


        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            using var scope = ServiceProvider.CreateScope();

            var messageBus = scope.ServiceProvider.GetRequiredService<ILocalMessageBus>();

            return await messageBus.Send(request);
        }
        public async Task SetupBeforeAnyTest()
        {
            await TestHarness.Start();
        }


        public async Task OnTearDown()
        {
            await TestHarness.Stop();
        }


        protected override void AfterAddApplication(IServiceCollection services)
        {

            services.Remove<ICurrentPrincipalAccessor>()
                .AddSingleton<ICurrentPrincipalAccessor, FakeCurrentPrincipalAccessor>();

        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }


    }
}