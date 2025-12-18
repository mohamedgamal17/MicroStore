using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Geographic.Application.Configuration;
using MicroStore.Geographic.Application.EntityFramework;
using Volo.Abp.AutoMapper;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace MicroStore.Geographic.Application
{
    [DependsOn(typeof(AbpAutoMapperModule),
        typeof(AbpFluentValidationModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpUnitOfWorkModule))]
    public class GeographicApplicationModule : AbpModule
    {
        public GeographicApplicationModule()
        {

        }

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();
            var appsettings = config.Get<ApplicationSettings>();
            context.Services.AddSingleton(appsettings);
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            Configure<AbpAutoMapperOptions>(opt => opt.AddMaps<GeographicApplicationModule>());

            context.Services.AddAbpDbContext<GeographicDbContext>(opt =>
            {
                opt.AddDefaultRepositories(true);
            });

            Configure<AbpDbContextOptions>(opt =>
            {
                opt.UseSqlServer(builder => builder.MigrationsAssembly(typeof(GeographicDbContext).Assembly.FullName));
            });

        }
    }
}