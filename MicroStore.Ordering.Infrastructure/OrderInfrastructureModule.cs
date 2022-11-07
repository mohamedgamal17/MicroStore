using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Ordering.Application;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Infrastructure.EntityFramework;
using Volo.Abp.Modularity;

namespace MicroStore.Ordering.Infrastructure
{
    [DependsOn(typeof(OrderApplicationModule))]
    public class OrderInfrastructureModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();

            ConfigureEntityFramework(context.Services, config);

            ConfigureMassTransit(context.Services, config);
        }

        private void ConfigureEntityFramework(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(builder => builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), cfg =>
            {
                cfg.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                cfg.MigrationsAssembly(typeof(OrderDbContext).Assembly.FullName);
            }));
        }

        private void ConfigureMassTransit(IServiceCollection services, IConfiguration configuration)
        {


            services.AddMassTransit(busRegisterConfig =>
            {

                busRegisterConfig.AddConsumers(typeof(OrderApplicationModule).Assembly);

                busRegisterConfig.AddActivities(typeof(OrderApplicationModule).Assembly);


                busRegisterConfig.AddSagaStateMachine<OrderStateMachine, OrderStateEntity>()
                    .EntityFrameworkRepository(efConfig =>
                    {
                        efConfig.UseSqlServer();
                        efConfig.ExistingDbContext<OrderDbContext>();
                    });



                busRegisterConfig.UsingRabbitMq((context, rabbitmqConfig) =>
                {
                    rabbitmqConfig.Host(configuration.GetValue<string>("MassTransitConfig:Host"), hostConfig =>
                    {
                        hostConfig.Username("MassTransitConfig:UserName");
                        hostConfig.Password("MassTransitConfig:Password");
                    });

                    rabbitmqConfig.ConfigureEndpoints(context);

                });
            });
        }
    }
}
