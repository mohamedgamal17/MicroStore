using Elastic.Clients.Elasticsearch;
using MassTransit;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using MicroStore.Catalog.Entities.ElasticSearch.Common;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using MicroStore.Catalog.Application.Operations.Extensions;
namespace MicroStore.Catalog.Application.Operations.Products
{
    public class ProductEventHandler :
        ILocalEventHandler<EntityCreatedEventData<Product>>,
        ILocalEventHandler<EntityUpdatedEventData<Product>>,
        ITransientDependency
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        private readonly IPublishEndpoint _publishEndPoint;
        private readonly IRepository<Product> _productRepository;
        public ProductEventHandler(ElasticsearchClient elasticsearchClient, IPublishEndpoint publishEndPoint, IRepository<Product> productRepository)
        {
            _elasticsearchClient = elasticsearchClient;
            _publishEndPoint = publishEndPoint;
            _productRepository = productRepository;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<Product> eventData)
        {
            var product = await _productRepository.SingleAsync(x => x.Id == eventData.Entity.Id);

            var elasticProduct = PrepareElasticProduct(product);

            var response = await _elasticsearchClient.IndexAsync(elasticProduct);

            response.ThrowIfFailure();

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

        public async Task HandleEventAsync(EntityUpdatedEventData<Product> eventData)
        {
            var productResponse = await _elasticsearchClient.GetAsync<ElasticProduct>(eventData.Entity.Id);

            productResponse.ThrowIfFailure();

            var product = await _productRepository.SingleAsync(x => x.Id == eventData.Entity.Id);

            var elasticProduct = PrepareElasticProduct(product, productResponse.Source?.ProductImages);

            var updateResponse = await _elasticsearchClient.IndexAsync(elasticProduct);

            updateResponse.ThrowIfFailure();

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

        private ElasticProduct PrepareElasticProduct(Product product, List<ElasticProductImage>? elasticProductImages = null)
        {

            var elasticProduct = new ElasticProduct
            {
                Id = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                ShortDescription = product.ShortDescription,
                LongDescription = product.LongDescription,
                IsFeatured = product.IsFeatured,
                Price = product.Price,
                OldPrice = product.OldPrice,
       

             

                Categories = product.Categories?.Select(x => new ElasticProductCategory
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),

                Manufacturers = product.Manufacturers?.Select(x => new ElasticProductManufacturer
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),

                Tags = product.Tags?.Select(x => new ElasticTag
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),

                SpecificationAttributes = product.SpecificationAttributes?.Select(x => new ElasticProductSpecificationAttribute
                {
                    Id = x.Id,
                    Name = x.Attribute.Name,
                    AttributeId = x.AttributeId,
                    OptionId = x.OptionId,
                    Value = x.Option.Name
                }).ToList(),
                CreationTime = product.CreationTime,
                CreatorId = product.CreatorId?.ToString(),
                LastModificationTime = product.LastModificationTime,
                LastModifierId = product.LastModifierId?.ToString(),
                DeletionTime = product.DeletionTime,
                DeleterId = product.DeleterId?.ToString(),
                IsDeleted = product.IsDeleted
            };


            if(product.Weight != null)
            {
                elasticProduct.Weight = new ElasticWeight
                {
                    Value = product.Weight.Value,
                    Unit = product.Weight.Unit
                };
            }

            if(product.Dimensions != null)
            {
                elasticProduct.Dimensions = new ElasticDimension
                {
                    Length = product.Dimensions.Length,
                    Width = product.Dimensions.Width,
                    Height = product.Dimensions.Height,
                    Unit = product.Dimensions.Unit
                };
            }



            if (elasticProduct.ProductImages != null)
            {
                foreach (var productImage in product.ProductImages)
                {

                    ElasticProductImage elasticProductImage = elasticProductImages?
                        .SingleOrDefault(x => x.Image == productImage.Image) ?? new ElasticProductImage();

                    elasticProductImage.Id = productImage.Id;
                    elasticProductImage.Image = productImage.Image;
                    elasticProductImage.DisplayOrder = productImage.DisplayOrder;
                    elasticProduct.ProductImages.Add(elasticProductImage);
                }

            }

            return elasticProduct;
        }

    }

}

