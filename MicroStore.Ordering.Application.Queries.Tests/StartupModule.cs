﻿using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Infrastructure;
using MicroStore.Ordering.Infrastructure.EntityFramework;
using MicroStore.TestBase.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using MicroStore.Ordering.Application.Abstractions.StateMachines;
using Microsoft.Extensions.Configuration;
using Respawn;
using Respawn.Graph;

namespace MicroStore.Ordering.Application.Queries.Tests
{
    [DependsOn(typeof(OrderApplicationModule),
        typeof(OrderInfrastructureModule),
        typeof(MediatorModule))]
    [DependsOn(typeof(AbpAutofacModule))]
    public class StartupModule : AbpModule
    {
        private readonly JsonSerializerSettings _jsonSerilizerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DomainModelContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
        };

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMassTransitTestHarness(busRegisterConfig =>
            {

                busRegisterConfig.AddConsumers(typeof(OrderApplicationModule).Assembly);

                busRegisterConfig.AddActivities(typeof(OrderApplicationModule).Assembly);

                busRegisterConfig.AddSagaStateMachine<OrderStateMachine, OrderStateEntity>()
                    .EntityFrameworkRepository(efConfig =>
                    {
                        efConfig.UseSqlServer();
                        efConfig.ExistingDbContext<OrderDbContext>();
                    });

                busRegisterConfig.UsingInMemory((context, inMemoryBusConfig) =>
                {
                    inMemoryBusConfig.ConfigureEndpoints(context);
                });
            });
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                dbContext.Database.Migrate();
                SeedOrdersData(dbContext);
            }
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            var config = context.ServiceProvider.GetRequiredService<IConfiguration>();

            var respawner =  Respawner.CreateAsync(config.GetConnectionString("DefaultConnection"), new RespawnerOptions
            {
                TablesToIgnore = new Table[]
                {
                    "__EFMigrationsHistory"
                }
            }).Result;


            respawner.ResetAsync(config.GetConnectionString("DefaultConnection")).Wait();
        }


        private void SeedOrdersData(OrderDbContext dbContext)
        {
            using (var stream = new StreamReader(@"Dummies\Orders.json"))
            {
                var json = stream.ReadToEnd();

                var data = JsonConvert.DeserializeObject<JsonWrapper<OrderStateEntity>>(json, _jsonSerilizerSettings);

                if(data != null)
                {
                    dbContext.AddRange(data.Data);
                }

                dbContext.SaveChanges();
            }
        }
    }
}