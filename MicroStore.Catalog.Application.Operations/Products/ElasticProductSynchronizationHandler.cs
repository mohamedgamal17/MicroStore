using Elastic.Clients.Elasticsearch;
using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Domain.ValueObjects;
using MicroStore.Catalog.Entities.ElasticSearch;
using MicroStore.Catalog.Entities.ElasticSearch.Common;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Catalog.Application.Operations.Products
{
    public class ElasticProductSynchronizationHandler :
        IConsumer<EntityCreatedEvent<ProductEto>>,
        IConsumer<EntityUpdatedEvent<ProductEto>>,
        IConsumer<ExtractImageFeaturesTask>,
        ITransientDependency
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        private readonly IImageService _imageService;

        private readonly IObjectMapper _objectMapper;

        private readonly IPublishEndpoint _publishEndPoint;

        private readonly ILogger<ElasticProductSynchronizationHandler> _logger;
        public ElasticProductSynchronizationHandler(ElasticsearchClient elasticsearchClient, IImageService imageService, IObjectMapper objectMapper, IPublishEndpoint publishEndPoint, ILogger<ElasticProductSynchronizationHandler> logger)
        {
            _elasticsearchClient = elasticsearchClient;
            _imageService = imageService;
            _objectMapper = objectMapper;
            _publishEndPoint = publishEndPoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<EntityCreatedEvent<ProductEto>> context)
        {
            var elasticEntity = _objectMapper.Map<ProductEto, ElasticProduct>(context.Message.Entity);

            var elasticProduct =  PreapreElasticProduct(context.Message.Entity);

            var response = await _elasticsearchClient.IndexAsync(elasticProduct);

            var unExtractedImages = elasticProduct.ProductImages.Where(x => x.Features == null).ToList();

            foreach (var productImage in unExtractedImages)
            {
                var task = new ExtractImageFeaturesTask
                {
                    ProductId = elasticProduct.Id,
                    ProductImageId = productImage.Id
                };

                await _publishEndPoint.Publish(task);
            }
        }

        public async Task Consume(ConsumeContext<EntityUpdatedEvent<ProductEto>> context)
        {
            var response = await _elasticsearchClient.GetAsync<ElasticProduct>(context.Message.Entity.Id);

            var elasticProduct =  PreapreElasticProduct(context.Message.Entity, response.Source?.ProductImages);

            await _elasticsearchClient.IndexAsync(elasticProduct);

            var unExtractedImages = elasticProduct.ProductImages.Where(x => x.Features == null).ToList();

            foreach (var productImage in unExtractedImages)
            {
                var task = new ExtractImageFeaturesTask
                {
                    ProductId = elasticProduct.Id,
                    ProductImageId = productImage.Id
                };

                await _publishEndPoint.Publish(task);
            }


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

        private ElasticProduct PreapreElasticProduct(ProductEto productEto, List<ElasticProductImage>? elasticProductImages = null)
        {

            var elasticProduct = new ElasticProduct
            {
                Id = productEto.Id,
                Sku = productEto.Sku,
                Name = productEto.Name,
                ShortDescription = productEto.ShortDescription,
                LongDescription = productEto.LongDescription,
                IsFeatured = productEto.IsFeatured,
                Price = productEto.Price,
                OldPrice = productEto.OldPrice,
                Weight = new ElasticWeight
                {
                    Value = productEto.Weight.Value,
                    Unit = Enum.Parse<WeightUnit>(productEto.Weight.Unit)
                },

                Dimensions = new ElasticDimension
                {
                    Length = productEto.Dimensions.Length,
                    Width = productEto.Dimensions.Width,
                    Height = productEto.Dimensions.Height,
                    Unit = Enum.Parse<DimensionUnit>(productEto.Dimensions.Unit)
                },

                ProductCategories = productEto.ProductCategories?.Select(x => new ElasticProductCategory
                {
                    Id = x.Id,
                    Name = x.Name,
                    CategoryId = x.CategoryId
                }).ToList(),

                ProductManufacturers = productEto.ProductManufacturers?.Select(x => new ElasticProductManufacturer
                {
                    Id = x.Id,
                    ManufacturerId = x.ManufacturerId,
                    Name = x.Name
                }).ToList(),

                ProductTags = productEto.ProductTags?.Select(x => new ElasticProductTag
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),

                SpecificationAttributes = productEto.SpecificationAttributes?.Select(x => new ElasticProductSpecificationAttribute
                {
                    Id = x.Id,
                    Name = x.Name,
                    AttributeId = x.AttributeId,
                    OptionId = x.OptionId,
                    Value = x.Value
                }).ToList(),
                CreatationTime = productEto.CreationTime,
                CreatorId = productEto.CreatorId?.ToString(),
                LastModificationTime = productEto.LastModificationTime,
                LastModifierId = productEto.LastModifierId?.ToString(),
                DeletionTime = productEto.DeletionTime,
                DeleterId = productEto.DeleterId?.ToString(),
                IsDeleted = productEto.IsDeleted
            };

            if(elasticProduct.ProductImages != null )
            {
                foreach (var productImage in productEto.ProductImages)
                {

                    ElasticProductImage elasticProductImage = elasticProductImages?
                        .SingleOrDefault(x => x.Image == productImage.Image) ?? new ElasticProductImage();

                    elasticProductImage.Id = productImage.Id;
                    elasticProductImage.Image = productImage.Image;
                    elasticProductImage.DisplayOrder = productImage.DisplayOrder;
                    elasticProductImage.CreatationTime = productImage.CreationTime;
                    elasticProductImage.CreatorId = productImage.CreatorId?.ToString();
                    elasticProductImage.LastModificationTime = productImage.LastModificationTime;
                    elasticProductImage.LastModifierId = productImage.LastModifierId?.ToString();
                    elasticProductImage.DeletionTime = productImage.DeletionTime;
                    elasticProductImage.DeleterId = productImage.DeleterId?.ToString();

                    elasticProduct.ProductImages.Add(elasticProductImage);
                }

            }


            return elasticProduct;
        }

    }

    public class ExtractImageFeaturesTask
    {
        public string ProductId { get; set; }
        public string ProductImageId { get; set; }

    }
}
