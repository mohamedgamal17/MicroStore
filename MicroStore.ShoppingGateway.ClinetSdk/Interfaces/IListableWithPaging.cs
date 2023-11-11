using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;

namespace MicroStore.ShoppingGateway.ClinetSdk.Interfaces
{
    public interface IListableWithPaging<TEntity, TOptions>
        where TEntity : class
        where TOptions : class
    {
        Task<PagedList<TEntity>> ListAsync(TOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default);
    }

    public interface INestedListableWithPaging<TEntity, TOptions> 
        where TEntity : class
        where TOptions : class
    {
        Task<PagedList<TEntity>> ListAsync(string parentId,TOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default);
    }
}
