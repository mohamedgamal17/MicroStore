using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Payment.Application.Configuration;
using MicroStore.Payment.Application.EntityFramework;
using MicroStore.Payment.Domain.Shared;
using System.Reflection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EventBus;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
namespace MicroStore.Payment.Application
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpEventBusModule),
        typeof(AbpDddApplicationModule),
        typeof(PaymentDomainSharedModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFluentValidationModule))]
    public class PaymentApplicationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();
            var appsettings = config.Get<ApplicationSettings>();
            context.Services.AddSingleton(appsettings);
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();

            Configure<AbpAutoMapperOptions>(opt => opt.AddMaps<PaymentApplicationModule>());

            ConfigureMassTransit(context.Services);

            ConfigureEntityFramework(context.Services);

        }

        private void ConfigureEntityFramework(IServiceCollection services)
        {

            services.AddAbpDbContext<PaymentDbContext>(opt =>
            {
                opt.AddDefaultRepositories(true);

            });

            Configure<AbpDbContextOptions>(opt =>
            {
                opt.UseSqlServer(builder => builder.MigrationsAssembly(typeof(PaymentDbContext).Assembly.FullName));
            });


        }
        private void ConfigureMassTransit(IServiceCollection services)
        {

            var appsettings = services.GetSingletonInstance<ApplicationSettings>();

            services.AddMassTransit((busRegisterConfig) =>
            {
                busRegisterConfig.AddConsumers(Assembly.GetExecutingAssembly());

                busRegisterConfig.UsingRabbitMq((context, rabbitMqConfig) =>
                {
                    rabbitMqConfig.Host(appsettings.MassTransit.Host, cfg =>
                    {
                        cfg.Username(appsettings.MassTransit.UserName);
                        cfg.Password(appsettings.MassTransit.Password);
                    });

                    rabbitMqConfig.ConfigureEndpoints(context);

                });
            });
        }

    }
}
