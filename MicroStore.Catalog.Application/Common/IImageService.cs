using MicroStore.Catalog.Domain.Entities;

namespace MicroStore.Catalog.Application.Common
{
    public interface IImageService
    {
        Task<List<ImageVector>> SearchByImage(byte[] buffer);
        Task IndexImageAsync(string productId, string imageId, string imageLink);
        Task<byte[]> GetImageBufferFromUrl(string url);
    }
}
