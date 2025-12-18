using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Profiling.Application.Configuration;
using MicroStore.Profiling.Application.EntityFramewrok;
using Volo.Abp.AutoMapper;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace MicroStore.Profiling.Application
{
    [DependsOn(typeof(AbpAutoMapperModule),
        typeof(AbpFluentValidationModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpUnitOfWorkModule))]
    public class ProfilingApplicationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();
            var appsettings = config.Get<ApplicationSettings>();
            context.Services.AddSingleton(appsettings);
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt => opt.AddMaps<ProfilingApplicationModule>());

            context.Services.AddAbpDbContext<ApplicationDbContext>(opt =>
            {
                opt.AddDefaultRepositories(true);
            });

            Configure<AbpDbContextOptions>(opt =>
            {
                opt.UseSqlServer(builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });

        }
    }
}
