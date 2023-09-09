using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.ProductTags;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.ProductTags
{
    public class ProductTagApplicationService : CatalogApplicationService , IProductTagApplicationService
    {
        private readonly IRepository<ProductTag> _productTagRepository;

        private readonly ICatalogDbContext _catalogDbContext;

        private readonly ElasticsearchClient _elasticSearchClient;
        public ProductTagApplicationService(IRepository<ProductTag> productTagRepository, ICatalogDbContext catalogDbContext, ElasticsearchClient elasticSearchClient)
        {
            _productTagRepository = productTagRepository;
            _catalogDbContext = catalogDbContext;
            _elasticSearchClient = elasticSearchClient;
        }

        public async Task<Result<ProductTagDto>> CreateAsync(ProductTagModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateProductTag(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ProductTagDto>(validationResult.Exception);
            }

            var productTag = new ProductTag
            {
                Name = model.Name,
                Description = model.Description,
            };

            await _productTagRepository.InsertAsync(productTag,cancellationToken: cancellationToken);

            return ObjectMapper.Map<ProductTag, ProductTagDto>(productTag);

        }

        public async Task<Result<ElasticProductTag>> GetAsync(string productTagId, CancellationToken cancellationToken = default)
        {
            var response = await _elasticSearchClient.GetAsync<ElasticProductTag>(productTagId, cancellationToken);

            if (!response.IsValidResponse)
            {
                return new Result<ElasticProductTag>(new EntityNotFoundException(typeof(ElasticProductTag), productTagId));
            }
           
            return response.Source!;
        }

        public async Task<Result<List<ElasticProductTag>>> ListAsync(CancellationToken cancellationToken = default)
        {
            var response = await _elasticSearchClient.SearchAsync<ElasticProductTag>(desc => desc
                .Query(qr => qr.MatchAll())
                .Size(5000)
            );

            if (!response.IsValidResponse)
            {
                return new List<ElasticProductTag>();
            }

            return response.Documents.ToList();

        }

        public async Task<Result<Unit>> RemoveAsync(string productTagId, CancellationToken cancellationToken = default)
        {
            var productTag = await _productTagRepository.SingleOrDefaultAsync(x => x.Id == productTagId, cancellationToken);

            if(productTag == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(ProductTag), productTagId));
            }

            await _productTagRepository.DeleteAsync(productTag, cancellationToken: cancellationToken);

            return Unit.Value;
        }

        public async Task<Result<ProductTagDto>> UpdateAsync(string productTagId, ProductTagModel model, CancellationToken cancellationToken = default)
        {
            var productTag = await _productTagRepository.SingleOrDefaultAsync(x => x.Id == productTagId, cancellationToken);

            if (productTag == null)
            {
                return new Result<ProductTagDto>(new EntityNotFoundException(typeof(ProductTag), productTagId));
            }

            var validationResult = await ValidateProductTag(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ProductTagDto>(validationResult.Exception);
            }

            productTag.Name = model.Name;

            productTag.Description = model.Description;

            await _productTagRepository.UpdateAsync(productTag, cancellationToken: cancellationToken);

            return ObjectMapper.Map<ProductTag, ProductTagDto>(productTag);
        }

        public async Task<Result<Unit>> ValidateProductTag(ProductTagModel model , string? productTagId = null, CancellationToken cancellationToken= default)
        {
            var query = await _productTagRepository.GetQueryableAsync();

            if(productTagId != null)
            {
                query = query.Where(x => x.Id == productTagId);
            }

            bool isProductTagNameExist = await query.AnyAsync(x => x.Name == model.Name, cancellationToken);

            if (isProductTagNameExist)
            {
                return new Result<Unit>(new UserFriendlyException($"Product tag name : {model.Name} is already exist"));
            }

            return Unit.Value;
        }

    }
}
