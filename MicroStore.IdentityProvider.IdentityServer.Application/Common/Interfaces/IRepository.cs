using System.Linq.Expressions;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> InsertAsync(TEntity entity , CancellationToken cancellationToken = default);

        Task<TEntity> UpdateAsync(TEntity entity , CancellationToken cancellationToken = default);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

    }
}
