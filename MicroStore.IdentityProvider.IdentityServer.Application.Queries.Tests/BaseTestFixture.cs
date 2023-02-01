using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.IdentityServer.Infrastructure.EntityFramework;
using MicroStore.TestBase;
using System.Linq.Expressions;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Queries.Tests
{
    [TestFixture]
    public abstract class BaseTestFixture : ApplicationTestBase<StartupModule>
    {

        private async Task<TResult> WithConfigurationContext<TResult>(Func<ApplicationConfigurationDbContext,Task<TResult>> action)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationConfigurationDbContext>();

                return await action(dbContext);
            }          
        }

        public Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return WithConfigurationContext<TEntity>((dbContext) => dbContext.Set<TEntity>().SingleAsync(predicate));
        }

        public Task<TEntity> FirstAsync<TEntity>() where TEntity : class
        {
            return WithConfigurationContext((dbContext) => dbContext.Set<TEntity>().FirstAsync());
        }
    }
}
