
namespace MicroStore.Client.PublicWeb.Infrastructure
{
    public interface IObjectStorageProvider
    {
        Task<string> CalculatePublicReferenceUrl(string objectName , CancellationToken cancellationToken = default);
        Task<Stream?> GetAsync(string objectName, CancellationToken cancellationToken = default);
        Task MigrateAsync(CancellationToken cancellationToken = default);
        Task SaveAsync(S3ObjectSaveArgs args, CancellationToken cancellationToken = default);
    }
}