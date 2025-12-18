using Microsoft.Extensions.DependencyInjection;
using MicroStore.TestBase.Fakes;
using Volo.Abp.Security.Claims;
using Volo.Abp;
using Volo.Abp.Testing;
using MicroStore.TestBase.Extensions;
using Microsoft.Extensions.Logging;
using MicroStore.TestBase.Utilites;
using Volo.Abp.Modularity;

namespace MicroStore.TestBase
{
    [Obsolete("Use ApplicationTestBase Instead")]
    public class QueryTestBase<TStartupModule> : ApplicationTestBase<TStartupModule> where TStartupModule : AbpModule
    {

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
