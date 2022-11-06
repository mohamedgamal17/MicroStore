using IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Security.Claims;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Testing;
using Volo.Abp.Uow;
using Volo.Abp;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using System.Data;
using MicroStore.TestBase.Extensions;
using MicroStore.TestBase.Fakes;

namespace MicroStore.TestBase
{
    [Obsolete]
    public abstract class ApplicationTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
           where TStartupModule : AbpModule
    {


        protected Guid UserId { get; private set; } = Guid.Empty;

        protected ClaimsPrincipal Principal { get; private set; } = new ClaimsPrincipal();

        public ApplicationTestBase() : base()
        {

        }


        public IServiceScope BeginScope()
        {
            return ServiceProvider.CreateScope();
        }

        [Obsolete]
        public void RunAsDefaultUser()
        {
            RunAsUser("UserName", new List<string> { "user" });
        }

        [Obsolete]
        public void RunAsAdmin()
        {
            RunAsUser("UserName", new List<string> { "admin" });
        }

        [Obsolete]
        public void RunAsUser(string username, List<string> roles)
        {
            UserId = Guid.NewGuid();

            ClaimsIdentity identities = new ClaimsIdentity();

            identities.AddClaim(new Claim(JwtClaimTypes.Subject, UserId.ToString()));
            identities.AddClaim(new Claim(JwtClaimTypes.Name, username));
            identities.AddClaim(new Claim(JwtClaimTypes.GivenName, "FakeUserName"));
            identities.AddClaim(new Claim(JwtClaimTypes.FamilyName, "FakeUserFamilyName"));


            roles.ForEach((role) =>
            {
                identities.AddClaim(new Claim(JwtClaimTypes.Role, role));
            });

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identities);
            Principal = claimsPrincipal;
        }


        [Obsolete]
        public void ResetUser()
        {
            UserId = Guid.Empty;
            Principal = new ClaimsPrincipal();
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            using var scope = ServiceProvider.CreateScope();

            var messageBus = scope.ServiceProvider.GetRequiredService<ILocalMessageBus>();

            return await messageBus.Send(request);
        }



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


        [Obsolete("Use WithUnitOfWork Instead")]
        public Task InsertAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            return WithUnitOfWork(async (sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                await repository.InsertAsync(entity);

            });
        }

        [Obsolete("Use WithUnitOfWork Instead")]
        public Task InsertRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity
        {
            return WithUnitOfWork(async (sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                await repository.InsertManyAsync(entities);
            });
        }

        [Obsolete("Use WithUnitOfWork Instead")]
        public Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            return WithUnitOfWork(async (sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                await repository.UpdateAsync(entity);

            });
        }

        [Obsolete("Use WithUnitOfWork Instead")]
        public Task UpdateRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity
        {
            return WithUnitOfWork(async (sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                await repository.UpdateManyAsync(entities);
            });
        }

        [Obsolete("Use WithUnitOfWork Instead")]
        public Task RemoveAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            return WithUnitOfWork(async (sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                await repository.DeleteAsync(entity);
            });
        }


        [Obsolete("Use WithUnitOfWork Instead")]
        public Task RemoveRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity
        {
            return WithUnitOfWork(async (sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                await repository.DeleteManyAsync(entities);
            });
        }


        [Obsolete("Use WithUnitOfWork Instead")]
        public Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity
            => WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                return repository.SingleAsync(predicate);
            });


        [Obsolete("Use WithUnitOfWork Instead")]
        public Task<TEntity> FirstAsync<TEntity>() where TEntity : class, IEntity
            => WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                return repository.FirstAsync<TEntity>();
            });

        [Obsolete("Use WithUnitOfWork Instead")]
        public Task<int> CountAsync<TEntity>() where TEntity : class, IEntity
            => WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                return repository.CountAsync();
            });

        [Obsolete("Use WithUnitOfWork Instead")]
        public Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity
            => WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                return repository.CountAsync(predicate);
            });



        [Obsolete("Use WithUnitOfWork Instead")]
        public Task<List<TEntity>> ListAsync<TEntity>() where TEntity : class, IEntity
            => WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<TEntity>>();

                return repository.GetListAsync();
            });


        protected override void AfterAddApplication(IServiceCollection services)
        {

            services.Remove<ICurrentPrincipalAccessor>()
                .AddSingleton<ICurrentPrincipalAccessor, FakeCurrentPrincipalAccessor>();

        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }


    }
}
