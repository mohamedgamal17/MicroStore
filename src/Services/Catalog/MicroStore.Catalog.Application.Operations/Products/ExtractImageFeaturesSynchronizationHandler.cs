using Elastic.Clients.Elasticsearch;
using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Catalog.Application.Operations.Products
{
    public class ExtractImageFeaturesSynchronizationHandler :
        IConsumer<ExtractImageFeaturesTask>,
        ITransientDependency
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        private readonly IImageService _imageService;

        private readonly ILogger<ExtractImageFeaturesSynchronizationHandler> _logger;

        public ExtractImageFeaturesSynchronizationHandler(ElasticsearchClient elasticsearchClient, IImageService imageService, ILogger<ExtractImageFeaturesSynchronizationHandler> logger)
        {
            _elasticsearchClient = elasticsearchClient;
            _imageService = imageService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ExtractImageFeaturesTask> context)
        {
            var response = await _elasticsearchClient.GetAsync<ElasticProduct>(context.Message.ProductId);

            if (response.IsValidResponse)
            {

                var elasticProduct = response.Source!;

                var productImage =  elasticProduct.ProductImages.Single(x => x.Id == context.Message.ProductImageId);

                var featureResult  = await _imageService.Descripe(productImage.Image);

                if (featureResult.IsFailure)
                {
                    _logger.LogWarning("Product with id : {id}.has follwing image feature extraction error : {error}", elasticProduct.Id, featureResult.Exception.Message);
                }
                else
                {
                    productImage.Features = featureResult.Value;

                    await _elasticsearchClient.IndexAsync(elasticProduct);
                }

                
            }
            
        }


    }

    public class ExtractImageFeaturesTask
    {
        public string ProductId { get; set; }
        public string ProductImageId { get; set; }

    }
}
