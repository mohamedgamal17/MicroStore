
using MicroStore.Catalog.Application.Abstractions.Common.Models;

namespace MicroStore.Catalog.Application.Abstractions.Common
{
    public interface IImageService
    {
        Task<ImageResult> SaveAsync(ImageModel imageModel, CancellationToken cancellationToken = default);
        Task<bool> IsValidLenght(ImageModel imageModel, CancellationToken cancellationToken = default);
    }
}
