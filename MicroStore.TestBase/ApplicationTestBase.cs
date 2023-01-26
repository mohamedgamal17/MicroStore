using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.TestBase.Extensions;
using MicroStore.TestBase.Fakes;
using MicroStore.TestBase.Utilites;
using System.Data;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Testing;
using Volo.Abp.Uow;

namespace MicroStore.TestBase
{
    public class ApplicationTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
         where TStartupModule : AbpModule
    {
        public async Task<ResponseResult<TResposne>> Send<TResposne>(IRequest<TResposne> request)
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
            services.AddSingleton<ILoggerFactory>(provider => new TestOutputLoggerFactory(true));


        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

    }
    
}
