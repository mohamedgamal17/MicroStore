using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.TestBase.Fakes;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Testing;
using Volo.Abp.Uow;
using Volo.Abp;
using System.Data;
using MicroStore.TestBase.Extensions;
using Castle.Core.Logging;
using MicroStore.Catalog.Domain.Tests.Utilites;
using Autofac.Core;

namespace MicroStore.TestBase
{
    public class MassTransitTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
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

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            using var scope = ServiceProvider.CreateScope();

            var messageBus = scope.ServiceProvider.GetRequiredService<ILocalMessageBus>();

            return await messageBus.Send(request);
        }


        public async Task WithUnitOfWork(Func<IServiceProvider, Task> func)
        {

            using (var scope = ServiceProvider.CreateScope())
            {
                var unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

                var options = new AbpUnitOfWorkOptions()
                {
                    IsTransactional = true,
                    IsolationLevel = IsolationLevel.ReadCommitted
                };

                using (var uow = unitOfWorkManager.Begin(options))
                {
                    await func(scope.ServiceProvider);

                    await uow.CompleteAsync();
                }
            }



        }


        public async Task<TResult> WithUnitOfWork<TResult>(Func<IServiceProvider, Task<TResult>> func)
        {
            using var scope = ServiceProvider.CreateScope();

            var unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

            var options = new AbpUnitOfWorkOptions()
            {
                IsTransactional = true,
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using (var uow = unitOfWorkManager.Begin(options))
            {
                var result = await func(scope.ServiceProvider);

                await uow.CompleteAsync();

                return result;
            }
        }


        protected override void AfterAddApplication(IServiceCollection services)
        {

            services.Remove<ICurrentPrincipalAccessor>()
                .AddSingleton<ICurrentPrincipalAccessor, FakeCurrentPrincipalAccessor>();

            services.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory>(provider => new TestOutputLoggerFactory(true));
  

        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
