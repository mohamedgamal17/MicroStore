using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.TestBase;
using Respawn;
using Respawn.Graph;
using System.Linq.Expressions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests
{
    [TestFixture]
    public abstract class BaseTestFixture : MassTransitTestBase<StartupModule>
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
        }






        [TearDown]
        protected async Task SetupAfterRunAnyTest()
        {
            var configuration = ServiceProvider.GetRequiredService<IConfiguration>();

            await Respawner.ResetAsync(configuration.GetConnectionString("DefaultConnection"));
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
    }
}
