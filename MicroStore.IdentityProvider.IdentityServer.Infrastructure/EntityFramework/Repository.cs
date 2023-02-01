using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;
using System.Linq.Expressions;

namespace MicroStore.IdentityProvider.IdentityServer.Infrastructure.EntityFramework
{
    public abstract class Repository<TContext ,TEntity> : IRepository<TEntity> 
        where TContext : DbContext
        where TEntity : class
    {
        protected readonly TContext DbContext;

        protected Repository(TContext dbContext)
        {
            DbContext = dbContext;
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return DbContext.Set<TEntity>().AnyAsync(expression, cancellationToken);
        }

        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbContext.Set<TEntity>().Remove(entity);

            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DbContext.Set<TEntity>().AddAsync(entity,cancellationToken);

            await DbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return DbContext.Set<TEntity>().SingleAsync(expression, cancellationToken);
        }

        public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return DbContext.Set<TEntity>().SingleOrDefaultAsync(expression, cancellationToken);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
             DbContext.Set<TEntity>().Update(entity);

            await DbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
