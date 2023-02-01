using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;
using MicroStore.TestBase;
using System.Linq.Expressions;
using Volo.Abp.Domain.Entities;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests
{
    public class BaseTestFixture : ApplicationTestBase<IdentityServerTestModule>
    {
        public async Task<TEntity> Insert<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = ServiceProvider.CreateScope();

            var repository = scope.ServiceProvider.GetRequiredService<IRepository<TEntity>>();

            return await repository.InsertAsync(entity);

        }




        public async Task<TEntity> Update<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = ServiceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<TEntity>>();

            return await repository.UpdateAsync(entity);

        }

        public async Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            using var scope = ServiceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<TEntity>>();
            return await repository.SingleAsync(expression);

        }

        public async Task<TEntity?> SingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            using var scope = ServiceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<TEntity>>();

            return await repository.SingleOrDefaultAsync(expression);

        }
    }
}
