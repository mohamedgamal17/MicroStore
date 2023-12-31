using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Common
{
    public interface IImageService
    {
        Task<Result<List<float>>> Descripe(string imageLink);


        Task<Result<byte[]>> GetImageBufferFromUrl(string url);
    }
}
