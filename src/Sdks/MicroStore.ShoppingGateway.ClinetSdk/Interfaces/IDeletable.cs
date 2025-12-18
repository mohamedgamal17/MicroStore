using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.ShoppingGateway.ClinetSdk.Interfaces
{
    public interface IDeletable
    {
        Task DeleteAsync(string id, RequestHeaderOptions options= null, CancellationToken cancellationToken = default);
    }

    public interface INestedDeletable
    {
        Task DeleteAsync(string parentId, string id , RequestHeaderOptions options = null, CancellationToken cancellationToken = default);
    }
}
