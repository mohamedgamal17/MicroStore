using Microsoft.Extensions.DependencyInjection;
using MicroStore.TestBase.Fakes;
using Volo.Abp.Security.Claims;
using Volo.Abp;
using Volo.Abp.Testing;
using MicroStore.TestBase.Extensions;
using Microsoft.Extensions.Logging;
using MicroStore.TestBase.Utilites;
using Volo.Abp.Modularity;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;

namespace MicroStore.TestBase
{
    [Obsolete("Use ApplicationTestBase Instead")]
    public class QueryTestBase<TStartupModule> : ApplicationTestBase<TStartupModule> where TStartupModule : AbpModule
    {
        public async Task<ResponseResult<TResposne>> Send<TResposne>(IRequest<TResposne> request)
        {
            using var scope = ServiceProvider.CreateScope();

            var messageBus = scope.ServiceProvider.GetRequiredService<ILocalMessageBus>();

            return await messageBus.Send(request);
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
