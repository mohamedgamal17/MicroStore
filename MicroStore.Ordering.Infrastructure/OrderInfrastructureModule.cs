using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Ordering.Application;
using MicroStore.Ordering.Application.Configuration;
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

            ConfigureEntityFramework(context.Services);

            ConfigureMassTransit(context.Services);
        }

        private void ConfigureEntityFramework(IServiceCollection services)
        {
            var appsettings = services.GetSingletonInstance<ApplicationSettings>();
            services.AddDbContext<OrderDbContext>(builder => builder.UseSqlServer(appsettings.ConnectionStrings.DefaultConnection, cfg =>
            {
                cfg.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                cfg.MigrationsAssembly(typeof(OrderDbContext).Assembly.FullName);
            }));
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            var appsettings = services.GetSingletonInstance<ApplicationSettings>();


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
                    rabbitmqConfig.Host(appsettings.MassTransit.Host, cfg =>
                    {
                        cfg.Username(appsettings.MassTransit.UserName);
                        cfg.Password(appsettings.MassTransit.Password);

                    });

                    rabbitmqConfig.ConfigureEndpoints(context);

                });
            });
        }
    }
}
