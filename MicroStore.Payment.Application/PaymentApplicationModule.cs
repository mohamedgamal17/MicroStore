using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Payment.Application.EntityFramework;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Domain.Shared.Domain;
using System.Reflection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace MicroStore.Payment.Application
{

    [DependsOn(typeof(InMemoryBusModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpEventBusModule),
        typeof(PaymentDomainSharedModule))]
    public class PaymentApplicationModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();

            ConfigureMassTransit(context.Services, config);

            ConfigureEntityFramework(context.Services);

        }

        private void ConfigureEntityFramework(IServiceCollection services)
        {

            services.AddAbpDbContext<PaymentDbContext>(opt =>
            {
                opt.AddDefaultRepositories(true);

                opt.AddRepository<PaymentRequest, PaymentRequestRepository > ();

            });

            Configure<AbpDbContextOptions>(opt =>
            {
                opt.UseSqlServer(builder => builder.MigrationsAssembly(typeof(PaymentDbContext).Assembly.FullName));
            });


        }
        private void ConfigureMassTransit(IServiceCollection services, IConfiguration configuration)
        {

            services.AddMassTransit((busRegisterConfig) =>
            {
                busRegisterConfig.AddConsumers(Assembly.GetExecutingAssembly());

                busRegisterConfig.UsingRabbitMq((context, rabbitMqConfig) =>
                {
                    rabbitMqConfig.Host(configuration.GetValue<string>("MassTransitConfig:Host"), cfg =>
                    {
                        cfg.Username(configuration.GetValue<string>("MassTransitConfig:UserName"));
                        cfg.Password(configuration.GetValue<string>("MassTransitConfig:Password"));
                    });

                });
            });
        }

    }
}
