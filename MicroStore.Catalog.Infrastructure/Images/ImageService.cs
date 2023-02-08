using Microsoft.Extensions.Logging;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using SixLabors.ImageSharp;
using MicroStore.Catalog.Application.Models;
using MicroStore.Catalog.Application.Common;

namespace MicroStore.Catalog.Infrastructure.Images
{
    public class ImageService : IImageService , ITransientDependency
    {
        private readonly IBlobContainer<ProductImageContainer> _blobContainer;

        private readonly ILogger<ImageService > _logger;

        private const int ImageMaxLenghtKB = 900;
        public ImageService(IBlobContainer<ProductImageContainer> blobContainer, ILogger<ImageService> logger)
        {
            _blobContainer = blobContainer;
            _logger = logger;
        }

        public async Task<ImageResult> SaveAsync(ImageModel model,CancellationToken cancellationToken = default)
        {
            try
            {

                using var stream = new MemoryStream(model.Data);

                var imageFormat = await Image.DetectFormatAsync(stream, cancellationToken);


                if(imageFormat == null)
                {
                    return new ImageResult
                    {
                        ImageLink = string.Empty,
                        IsValid = false
                    };
                }

                string imageLink = string.Format(@"{0}_{1}.{2}", DateTime.Now.Millisecond, model.FileName,imageFormat.FileExtensions.First());

                await _blobContainer.SaveAsync(imageLink, model.Data, true, cancellationToken: cancellationToken);

                return new ImageResult
                {
                    ImageLink = imageLink,
                    IsValid = true
                };

            }
            catch(NotSupportedException)
            {
                return new ImageResult
                {
                    ImageLink = string.Empty,
                    IsValid = false
                };
            }
            
        }

       

        public  Task<bool> IsValidLenght(ImageModel model, CancellationToken cancellationToken = default)
        {
            decimal lenght =(decimal) model.Data.Length / 1024 ;

            if(lenght > ImageMaxLenghtKB || lenght <= 0)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }


      
    }
}

