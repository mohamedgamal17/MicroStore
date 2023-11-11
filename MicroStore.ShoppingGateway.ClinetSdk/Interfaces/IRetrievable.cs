using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.ShoppingGateway.ClinetSdk.Interfaces
{
    public interface IRetrievable<TEntity>
    {
        Task<TEntity> GetAsync(string id, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default);
    }
    public interface INestedRetrievable<TEntity>
    {
        Task<TEntity> GetAsync(string parentId, string id, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default);
    }
}
