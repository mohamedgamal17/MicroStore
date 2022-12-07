using Microsoft.Extensions.Logging;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Catalog.Infrastructure.Images
{
    public class ImageService : IImageService , ITransientDependency
    {
        private readonly IBlobContainer<ProductImageContainer> _blobContainer;

        private readonly ILogger<ImageService > _logger;
        public ImageService(IBlobContainer<ProductImageContainer> blobContainer, ILogger<ImageService> logger)
        {
            _blobContainer = blobContainer;
            _logger = logger;
        }

        public async Task<ImageResult> SaveAsync(ImageModel imageModel,CancellationToken cancellationToken = default)
        {
            try
            {
                string imageLink = string.Format("{0}_{1}", DateTime.Now.Millisecond, imageModel.FileName);

                await _blobContainer.SaveAsync(imageLink, imageModel.Data, true, cancellationToken: cancellationToken);

                return new ImageResult
                {
                    ImageLink = imageLink
                };

            }
            catch(Exception ex)
            {
                _logger.LogException(ex);
                throw ex;
            }
            
        }
    }
}

