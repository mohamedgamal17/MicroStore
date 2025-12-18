using MicroStore.ShoppingGateway.ClinetSdk.Common;
namespace MicroStore.ShoppingGateway.ClinetSdk.Interfaces
{
    public interface IUpdateable<TEntity, TOptions>
        where TEntity : class
        where TOptions : class
    {
        Task<TEntity> UpdateAsync(string id,  TOptions options = null,RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default);
    }

    public interface IUpdateableResource<TEntity, TOptions>
        where TEntity : class
        where TOptions : class
    {
        Task<TEntity> UpdateAsync(TOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default);
    }

    public interface INestedUpdateable<TEntity, TOptions>
        where TEntity : class
        where TOptions : class
    {
        Task<TEntity> UpdateAsync(string parentId ,string id, TOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default);
    }

    public interface INestedUpdateableResource<TEntity, TOptions>
        where TEntity : class
        where TOptions : class
    {
        Task<TEntity> UpdateAsync( string id, TOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default);
    }

}
