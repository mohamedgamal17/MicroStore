using Elastic.Clients.Elasticsearch;
using Emgu.CV;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Catalog.Infrastructure.Services
{
    public class ImageService : IImageService , ITransientDependency
    {
        private readonly IImageDescriptor _imageDescriptor;

        private readonly ElasticsearchClient _elasticSearchClient;

        public ImageService(IImageDescriptor imageDescriptor, ElasticsearchClient elasticSearchClient)
        {
            _imageDescriptor = imageDescriptor;
            _elasticSearchClient = elasticSearchClient;
        }

        public async Task<byte[]> GetImageBufferFromUrl(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var memoryStream = new MemoryStream();

                var httpResponse = await httpClient.GetAsync(url);

                await httpResponse.Content.CopyToAsync(memoryStream);

                return memoryStream.ToArray();
            }
        }

        public async Task IndexImageAsync(string productId, string imageId, string imageLink)
        {
            var buffer = await GetImageBufferFromUrl(imageLink);

            var imageMat = new Mat();

            CvInvoke.Imdecode(buffer, Emgu.CV.CvEnum.ImreadModes.Color, imageMat);

            CvInvoke.CvtColor(imageMat, imageMat, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

            var features = _imageDescriptor.Descripe(imageMat);

            ElasticImageVector imageVector = new ElasticImageVector
            {
                ProductId = productId,
                ImageId = imageId,
                Features = features
            };


            await _elasticSearchClient.IndexAsync(imageVector);
        }

        public async Task<List<ElasticImageVector>> SearchByImage(byte[] buffer)
        {
            var imageMat = new Mat();

            CvInvoke.Imdecode(buffer, Emgu.CV.CvEnum.ImreadModes.Color, imageMat);

            CvInvoke.CvtColor(imageMat, imageMat, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

            var features = _imageDescriptor.Descripe(imageMat);

            var response = await _elasticSearchClient.SearchAsync<ElasticImageVector>(sr => sr
                     .Knn(k => k
                         .QueryVector(features)
                         .k(100)
                         .NumCandidates(100)
                     )
                 );


            if (response.IsValidResponse)
            {
                return response.Documents.ToList();
            }

            return new List<ElasticImageVector>();
        }
    }
}
