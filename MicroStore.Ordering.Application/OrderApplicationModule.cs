using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Ordering.Application.Configuration;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.AutoMapper;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
namespace MicroStore.Ordering.Application
{

    [DependsOn(typeof(AbpAuthorizationModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpUnitOfWorkModule),
        typeof(AbpFluentValidationModule),
        typeof(AbpDddApplicationModule))]
    public class OrderApplicationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();
            var appsettings = config.Get<ApplicationSettings>();
            context.Services.AddSingleton(appsettings);
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt => opt.AddMaps<OrderApplicationModule>());
        }
    }
}
