using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.ShoppingGateway.ClinetSdk.Interfaces
{
    public interface ICreatable <TEntity, TOptions>
        where TEntity : class
        where TOptions : class
    {
        Task<TEntity> CreateAsync(TOptions options = null , RequestHeaderOptions requestHeaderOptions= null, CancellationToken cancellationToken = default);
    }


    public interface INestedCreatable<TEntity , TOptions>
        where TEntity : class
        where TOptions : class
    {
        Task<TEntity> CreateAsync(string parentId, TOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default);
    }


}
