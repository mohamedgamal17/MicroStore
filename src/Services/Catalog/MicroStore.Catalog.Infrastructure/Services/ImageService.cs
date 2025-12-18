using Emgu.CV;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Application.Abstractions.Common;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Catalog.Infrastructure.Services
{
    public class ImageService : IImageService , ITransientDependency
    {
        private readonly IImageDescriptor _imageDescriptor;


        private List<string> _imageContentType = new List<string>
        {
            "image/jpg",
            "image/jpeg",
            "image/pjpeg",
            "image/gif",
            "image/x-png",
            "image/png"
        };

        public ImageService(IImageDescriptor imageDescriptor)
        {
            _imageDescriptor = imageDescriptor;
        }

        public async Task<Result<List<float>>> Descripe(string imageLink)
        {
            var result = await GetImageBufferFromUrl(imageLink);

            if (result.IsFailure)
            {
                return new Result<List<float>>(result.Exception);
            }

            byte[] buffer = result.Value;

            var imageMat = new Mat();

            CvInvoke.Imdecode(buffer, Emgu.CV.CvEnum.ImreadModes.Color, imageMat);

            CvInvoke.CvtColor(imageMat, imageMat, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

            var features = _imageDescriptor.Descripe(imageMat);

            return features;
        }

        public async Task<Result<byte[]>> GetImageBufferFromUrl(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var memoryStream = new MemoryStream();

                var httpResponse = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead) ;

                if (!httpResponse.IsSuccessStatusCode)
                {
                    return new Result<byte[]>(new UserFriendlyException("Cannot retrive image from desired url"));
                }

                if (!_imageContentType.Contains(httpResponse.Content.Headers.ContentType?.MediaType!))
                {
                    return new Result<byte[]>(new UserFriendlyException("Invalid image uri link"));
                }


                await httpResponse.Content.CopyToAsync(memoryStream);

                return memoryStream.ToArray();
            }
        }
    }
}
