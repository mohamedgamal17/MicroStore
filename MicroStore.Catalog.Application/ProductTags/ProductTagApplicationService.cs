using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Application.Abstractions.ProductTags;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.ProductTags
{
    public class ProductTagApplicationService : CatalogApplicationService , IProductTagApplicationService
    {
        private readonly IRepository<Tag> _productTagRepository;

        private readonly ICatalogDbContext _catalogDbContext;

        private readonly ElasticsearchClient _elasticSearchClient;
        public ProductTagApplicationService(IRepository<Tag> productTagRepository, ICatalogDbContext catalogDbContext, ElasticsearchClient elasticSearchClient)
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

            var productTag = new Tag
            {
                Name = model.Name,
                Description = model.Description,
            };

            await _productTagRepository.InsertAsync(productTag,cancellationToken: cancellationToken);

            return ObjectMapper.Map<Tag, ProductTagDto>(productTag);

        }

        public async Task<Result<ProductTagDto>> GetAsync(string productTagId, CancellationToken cancellationToken = default)
        {
            var response = await _elasticSearchClient.GetAsync<ElasticTag>(productTagId, cancellationToken);

            if (!response.IsValidResponse)
            {
                return new Result<ProductTagDto>(new EntityNotFoundException(typeof(ElasticTag), productTagId));
            }
           
            return ObjectMapper.Map<ElasticTag, ProductTagDto>(response.Source!);
        }

        public async Task<Result<List<ProductTagDto>>> ListAsync(CancellationToken cancellationToken = default)
        {
            var response = await _elasticSearchClient.SearchAsync<ElasticTag>(desc => desc
                .Query(qr => qr.MatchAll())
                .Size(5000)
            );

            if (!response.IsValidResponse)
            {
                return new List<ProductTagDto>();
            }

            var result = response.Documents.ToList();

            return ObjectMapper.Map<List<ElasticTag>, List<ProductTagDto>>(result);
        }

        public async Task<Result<Unit>> RemoveAsync(string productTagId, CancellationToken cancellationToken = default)
        {
            var productTag = await _productTagRepository.SingleOrDefaultAsync(x => x.Id == productTagId, cancellationToken);

            if(productTag == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(Tag), productTagId));
            }

            await _productTagRepository.DeleteAsync(productTag, cancellationToken: cancellationToken);

            return Unit.Value;
        }

        public async Task<Result<ProductTagDto>> UpdateAsync(string productTagId, ProductTagModel model, CancellationToken cancellationToken = default)
        {
            var productTag = await _productTagRepository.SingleOrDefaultAsync(x => x.Id == productTagId, cancellationToken);

            if (productTag == null)
            {
                return new Result<ProductTagDto>(new EntityNotFoundException(typeof(Tag), productTagId));
            }

            var validationResult = await ValidateProductTag(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ProductTagDto>(validationResult.Exception);
            }

            productTag.Name = model.Name;

            productTag.Description = model.Description;

            await _productTagRepository.UpdateAsync(productTag, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Tag, ProductTagDto>(productTag);
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
