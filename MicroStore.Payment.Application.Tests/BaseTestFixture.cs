using MicroStore.TestBase;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.Linq.Expressions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Payment.Application.Tests
{
    [TestFixture]
    public class BaseTestFixture : MassTransitTestBase<PaymentApplicationTestModule>
    {
        [OneTimeSetUp]
        protected async Task SetupBeforeAllTests()
        {
            await StartMassTransit();
        }

        [OneTimeTearDown]
        protected async Task SetupAfterRunAllTest()
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
