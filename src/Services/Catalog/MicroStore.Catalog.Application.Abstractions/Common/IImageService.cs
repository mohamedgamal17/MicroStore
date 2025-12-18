using MicroStore.BuildingBlocks.Utils.Results;
namespace MicroStore.Catalog.Application.Abstractions.Common
{
    public interface IImageService
    {
        Task<Result<List<float>>> Descripe(string imageLink);


        Task<Result<byte[]>> GetImageBufferFromUrl(string url);
    }
}
