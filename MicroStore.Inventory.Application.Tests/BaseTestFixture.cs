using IdentityModel;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.TestBase;
using Respawn;
using Respawn.Graph;
using System.Linq.Expressions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Inventory.Application.Tests
{
    [TestFixture]
    public abstract class BaseTestFixture : MassTransitTestBase<InventoryApplicationTestModule>
    {



        [OneTimeSetUp]
        protected  async Task SetupBeforeAllTests()
        {       
            await StartMassTransit();
        }

        [OneTimeTearDown]
        protected async Task SetupAfterRunAnyTest()
        {
            await StopMassTransit();
        }


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

        public Task<long> Count<TEntity>() where TEntity : class, IEntity
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                return repository.GetCountAsync();
            });
        }

    }
}
