using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.TestBase;
using NUnit.Framework;
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

        [OneTimeSetUp]
        protected async Task SetupBeforeAllTests()
        {         
            await StartMassTransit();
        }

        [OneTimeTearDown]
        protected async Task SetupAfterRunAnyTest()
        {
            await StopMassTransit();
        }

        public async Task<TEntity> Insert<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            using var dbContext = GetRequiredService<DbContext>();

            await dbContext.Set<TEntity>().AddAsync(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<TEntity>> InsertMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity
        {
            using var dbContext = GetRequiredService<DbContext>();

            await dbContext.Set<TEntity>().AddRangeAsync(entities);

            await dbContext.SaveChangesAsync();

            return entities;
        }


        public async Task<TEntity> Update<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            using var dbContext = GetRequiredService<DbContext>();

            dbContext.Set<TEntity>().Update(entity);

            await dbContext.SaveChangesAsync();

            return entity;

        }

        public async Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, IEntity
        {
            using var dbContext = GetRequiredService<DbContext>();

           return await dbContext.Set<TEntity>().SingleAsync(expression);
        }

        public async Task<TEntity?> SingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, IEntity
        {
            using var dbContext = GetRequiredService<DbContext>();

            return await dbContext.Set<TEntity>().SingleOrDefaultAsync(expression);
        }

        public async Task<TEntity> First<TEntity>() where TEntity : class , IEntity
        {
            using var dbContext = GetRequiredService<DbContext>();

            return await dbContext.Set<TEntity>().FirstAsync();
        }

        public async Task<TEntity?> FirstOrDefaut<TEntity>() where TEntity : class, IEntity
        {
            using var dbContext = GetRequiredService<DbContext>();

            return await dbContext.Set<TEntity>().FirstOrDefaultAsync();
        }
    }
}
