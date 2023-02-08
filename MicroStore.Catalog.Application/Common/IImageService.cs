using MicroStore.Catalog.Application.Models;

namespace MicroStore.Catalog.Application.Common
{
    public interface IImageService
    {
        Task<ImageResult> SaveAsync(ImageModel imageModel, CancellationToken cancellationToken = default);
        Task<bool> IsValidLenght(ImageModel imageModel, CancellationToken cancellationToken = default);
    }
}
