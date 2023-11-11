using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.ShoppingGateway.ClinetSdk.Interfaces
{
    public interface IListable<TEntity, TOptions>
        where TOptions : class
    {
        Task<List<TEntity>> ListAsync(TOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default);
    }

    public interface IListable<TEntity>
    {
        Task<List<TEntity>> ListAsync(RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default);
    }
    public interface INestedListable<TEntity>
    {
        Task<List<TEntity>> ListAsync(string parentId,  RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default);
    }

}
