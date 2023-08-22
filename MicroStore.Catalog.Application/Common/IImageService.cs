using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Common
{
    public interface IImageService
    {
        Task<List<ElasticImageVector>> SearchByImage(byte[] buffer);
        Task IndexImageAsync(string productId, string imageId, string imageLink);
        Task<byte[]> GetImageBufferFromUrl(string url);
    }
}
