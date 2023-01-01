using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MicroStore.Shipping.PluginInMemoryTest.Utilites;
using System.Data;
using System.Linq.Expressions;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Testing;
using Volo.Abp.Uow;

namespace MicroStore.Shipping.PluginInMemoryTest
{
    public abstract class PluginTestFixture<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : AbpModule
    {
        public async Task WithUnitOfWork(Func<IServiceProvider, Task> func)
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


        public async Task<TResult> WithUnitOfWork<TResult>(Func<IServiceProvider, Task<TResult>> func)
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


        public Task<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            return WithUnitOfWork(async (sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                return await repository.InsertAsync(entity);

            });
        }

        public Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            return WithUnitOfWork(async (sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                return await repository.UpdateAsync(entity);

            });
        }

        public Task<int> CountAsync<TEntity>() where TEntity : class, IEntity
         => WithUnitOfWork((sp) =>
         {
             var repository = sp.GetRequiredService<IRepository<TEntity>>();

             return repository.CountAsync();
         });

        public Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity
            => WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                return repository.SingleAsync(predicate);
            });


        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {

            services.AddSingleton<ILoggerFactory>(provider => new TestOutputLoggerFactory(true));
        }


    }
}
