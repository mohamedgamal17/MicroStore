using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MicroStore.TestBase.Extensions;
using MicroStore.TestBase.Fakes;
using MicroStore.TestBase.Utilites;
using System.Data;
using System.Linq.Expressions;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Testing;
using Volo.Abp.Uow;
namespace MicroStore.TestBase
{
    public class ApplicationTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
         where TStartupModule : AbpModule
    {


        public virtual async Task<TEntity> Insert<TEntity>(TEntity entity) where TEntity : class
        {
            using var dbContext = GetRequiredService<DbContext>();

            await dbContext.Set<TEntity>().AddAsync(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> InsertMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            using var dbContext = GetRequiredService<DbContext>();

            await dbContext.Set<TEntity>().AddRangeAsync(entities);

            await dbContext.SaveChangesAsync();

            return entities;
        }


        public virtual async Task<TEntity> Update<TEntity>(TEntity entity) where TEntity : class
        {
            using var dbContext = GetRequiredService<DbContext>();

            dbContext.Set<TEntity>().Update(entity);

            await dbContext.SaveChangesAsync();

            return entity;

        }

        public virtual async Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> expression , params Expression<Func<TEntity, object>>[] properties) where TEntity : class
        {
            using var dbContext = GetRequiredService<DbContext>();

            var query = dbContext.Set<TEntity>().AsQueryable();

            foreach (var prop in properties)
            {
               query = query.Include(prop);
            }

            return await query.SingleAsync(expression);
        }

        public virtual async Task<TEntity?> SingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] properties) where TEntity : class
        {
            using var dbContext = GetRequiredService<DbContext>();

            var query = dbContext.Set<TEntity>().AsQueryable();

            foreach (var prop in properties)
            {
                query = query.Include(prop);
            }

            return await query.SingleOrDefaultAsync(expression);
        }

        public virtual async Task<TEntity> FirstAsync<TEntity>() where TEntity : class
        {
            using var dbContext = GetRequiredService<DbContext>();

            return await dbContext.Set<TEntity>().FirstAsync();
        }

        public virtual async Task<TEntity?> FirstOrDefaut<TEntity>() where TEntity : class
        {
            using var dbContext = GetRequiredService<DbContext>();

            return await dbContext.Set<TEntity>().FirstOrDefaultAsync();
        }


        public virtual async Task WithUnitOfWork(Func<IServiceProvider, Task> func)
        {

            using (var scope = ServiceProvider.CreateScope())
            {
                var unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

                var options = new AbpUnitOfWorkOptions()
                {
                    IsTransactional = true,
                    IsolationLevel = IsolationLevel.ReadCommitted
                };

                using (var uow = unitOfWorkManager.Begin(options))
                {
                    await func(scope.ServiceProvider);

                    await uow.CompleteAsync();
                }
            }
        }

        public virtual async Task<TResult> WithUnitOfWork<TResult>(Func<IServiceProvider, Task<TResult>> func)
        {
            using var scope = ServiceProvider.CreateScope();

            var unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

            var options = new AbpUnitOfWorkOptions()
            {
                IsTransactional = true,
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using (var uow = unitOfWorkManager.Begin(options))
            {
                var result = await func(scope.ServiceProvider);

                await uow.CompleteAsync();

                return result;
            }
        }


        protected override void AfterAddApplication(IServiceCollection services)
        {

            services.Remove<ICurrentPrincipalAccessor>()
                .AddSingleton<ICurrentPrincipalAccessor, FakeCurrentPrincipalAccessor>();
            services.AddSingleton<ILoggerFactory>(provider => new TestOutputLoggerFactory(true));


        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

    }
    
}
