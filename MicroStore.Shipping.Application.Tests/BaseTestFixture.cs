using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Tests.Fakes;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.TestBase;
using Respawn;
using Respawn.Graph;
using System.Linq.Expressions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Shipping.Application.Tests
{
    public class BaseTestFixture : MassTransitTestBase<ShippingApplicationTestModule>
    {
        public Respawner Respawner { get; set; }

        [OneTimeSetUp]
        protected async Task SetupBeforeAllTests()
        {

            var configuration = ServiceProvider.GetRequiredService<IConfiguration>();

            Respawner = await Respawner.CreateAsync(configuration.GetConnectionString("DefaultConnection"), new RespawnerOptions
            {
                TablesToIgnore = new Table[]
                {
                   "__EFMigrationsHistory"
                }
            });

            await Insert(new ShippingSystem
            {
                Name = FakeConst.ActiveSystem,
                DisplayName = FakeConst.ActiveSystem,
                Image = Guid.NewGuid().ToString(),
                IsEnabled = true
            });

            await Insert(new ShippingSystem
            {
                Name = FakeConst.NotActiveSystem,
                DisplayName = FakeConst.NotActiveSystem,
                Image = Guid.NewGuid().ToString(),
                IsEnabled = false
            });

            var settings = new ShippingSettings
            {
                DefaultShippingSystem = FakeConst.ActiveSystem,
                Location = new AddressSettings
                {
                    CountryCode = "US",
                    State = "CA",
                    City = "San Jose",
                    AddressLine1 = "525 S Winchester Blvd",
                    AddressLine2 = "525 S Winchester Blvd",
                    Name = "Jane Doe",
                    Phone = "555-555-5555",
                    PostalCode = "95128",
                    Zip = "90241"
                }
            };

            await UpdateSettings(settings);

            await StartMassTransit();
        }

        [OneTimeTearDown]
        protected async Task SetupAfterAllTests()
        {
            await StopMassTransit();
        }

   
        public Task<TEntity> Insert<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                return repository.InsertAsync(entity);
            });
        }


        public Task<TEntity> Update<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                return repository.UpdateAsync(entity);
            });
        }
  
        public Task<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, IEntity
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                return repository.SingleAsync(expression);
            });
        }

        public Task<TEntity> First<TEntity>() where TEntity : class, IEntity
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                return repository.FirstAsync();
            });
        }

        public async Task<ShippingSettings> TryToGetSettings()
        {
            var repository = ServiceProvider.GetRequiredService<ISettingsRepository>();

            return await repository.TryToGetSettings<ShippingSettings>(SettingsConst.ProviderKey) ?? new ShippingSettings();
        }

        public async Task<ShippingSettings> UpdateSettings(ShippingSettings settings)
        {
            var repository = ServiceProvider.GetRequiredService<ISettingsRepository>();

            await repository.TryToUpdateSettrings(settings);

            return await TryToGetSettings();
        }
    }
}
