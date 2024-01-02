using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Application.Abstractions.SpecificationAttributes;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.SpecificationAttributes
{
    public class SpecificationAttributeApplicationService : CatalogApplicationService, ISpecificationAttributeApplicationService
    {
        private readonly IRepository<SpecificationAttribute> _specificationAttributeRepository;

        private readonly ICatalogDbContext _catalogDbContext;

        private readonly ElasticsearchClient _elasticSearchClient;
        public SpecificationAttributeApplicationService(IRepository<SpecificationAttribute> specificationAttributeRepository, ICatalogDbContext catalogDbContext, ElasticsearchClient elasticSearchClient)
        {
            _specificationAttributeRepository = specificationAttributeRepository;
            _catalogDbContext = catalogDbContext;
            _elasticSearchClient = elasticSearchClient;
        }

        public async Task<Result<SpecificationAttributeDto>> CreateAsync(SpecificationAttributeModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateSpecifcationAttribute(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<SpecificationAttributeDto>(validationResult.Exception);
            }


            var attribute = new SpecificationAttribute();

            PrepareSpecifcationAttribute(attribute, model);

            await _specificationAttributeRepository.InsertAsync(attribute);

            return ObjectMapper.Map<SpecificationAttribute, SpecificationAttributeDto>(attribute);
           
        }

        public async Task<Result<SpecificationAttributeDto>> UpdateAsync(string attributeId, SpecificationAttributeModel model, CancellationToken cancellationToken = default)
        {
            var query = await _specificationAttributeRepository.WithDetailsAsync(x => x.Options);

            var attribute = await query.SingleOrDefaultAsync(x => x.Id == attributeId , cancellationToken);

            if(attribute == null)
            {
                return new Result<SpecificationAttributeDto>(new EntityNotFoundException(typeof(SpecificationAttribute), attributeId));
            }

            var validationResult = await ValidateSpecifcationAttribute(model,attributeId ,cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<SpecificationAttributeDto>(validationResult.Exception);
            }

            PrepareSpecifcationAttribute(attribute, model);

            await _specificationAttributeRepository.UpdateAsync(attribute, cancellationToken: cancellationToken);

            return ObjectMapper.Map<SpecificationAttribute, SpecificationAttributeDto>(attribute);
        }

        public async Task<Result<Unit>> RemoveAsync(string attributeId, CancellationToken cancellationToken = default)
        {
            var query = await _specificationAttributeRepository.WithDetailsAsync(x => x.Options);

            var attribute = await query.SingleOrDefaultAsync(x => x.Id == attributeId, cancellationToken);

            if (attribute == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(SpecificationAttribute), attributeId));
            }

            await _specificationAttributeRepository.DeleteAsync(attribute, cancellationToken: cancellationToken);

            return Unit.Value;
        }

        public async Task<Result<SpecificationAttributeDto>> CreateOptionAsync(string attributeId, SpecificationAttributeOptionModel model, CancellationToken cancellationToken = default)
        {
            var query = await _specificationAttributeRepository.WithDetailsAsync(x => x.Options);

            var attribute = await query.SingleOrDefaultAsync(x => x.Id == attributeId, cancellationToken);

            if (attribute == null)
            {
                return new Result<SpecificationAttributeDto>(new EntityNotFoundException(typeof(SpecificationAttribute), attributeId));
            }

            var validationResult = await ValidateSpecifcationAttributeOption(attributeId,model, cancellationToken: cancellationToken);


            if (validationResult.IsFailure)
            {
                return new Result<SpecificationAttributeDto>(validationResult.Exception);
            }

            var option = new SpecificationAttributeOption
            {
                Name = model.Name
            };

            attribute.Options.Add(option);

            await _specificationAttributeRepository.UpdateAsync(attribute);

            return ObjectMapper.Map<SpecificationAttribute, SpecificationAttributeDto>(attribute);
        }

        public async Task<Result<SpecificationAttributeDto>> UpdateOptionAsync(string attributeId, string optionId, SpecificationAttributeOptionModel model, CancellationToken cancellationToken = default)
        {
            var query = await _specificationAttributeRepository.WithDetailsAsync(x => x.Options);

            var attribute = await query.SingleOrDefaultAsync(x => x.Id == attributeId, cancellationToken);

            if (attribute == null)
            {
                return new Result<SpecificationAttributeDto>(new EntityNotFoundException(typeof(SpecificationAttribute), attributeId));
            }

            var option = attribute.Options.SingleOrDefault(x => x.Id == optionId);

            if(option == null)
            {
                return new Result<SpecificationAttributeDto>(new EntityNotFoundException(typeof(SpecificationAttributeOption), optionId));
            }

            var validationResult = await ValidateSpecifcationAttributeOption(attributeId, model, optionId, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<SpecificationAttributeDto>(validationResult.Exception);
            }

            option.Name = model.Name;

            await _specificationAttributeRepository.UpdateAsync(attribute, cancellationToken: cancellationToken);

            return ObjectMapper.Map<SpecificationAttribute, SpecificationAttributeDto>(attribute);
        }


        public async Task<Result<SpecificationAttributeDto>> RemoveOptionAsync(string attributeId, string optionId, CancellationToken cancellationToken = default)
        {
            var query = await _specificationAttributeRepository.WithDetailsAsync(x => x.Options);

            var attribute = await query.SingleOrDefaultAsync(x => x.Id == attributeId, cancellationToken);

            if (attribute == null)
            {
                return new Result<SpecificationAttributeDto>(new EntityNotFoundException(typeof(SpecificationAttribute), attributeId));
            }

            var option = attribute.Options.SingleOrDefault(x => x.Id == optionId);

            if (option == null)
            {
                return new Result<SpecificationAttributeDto>(new EntityNotFoundException(typeof(SpecificationAttributeOption), optionId));
            }

            attribute.Options.Remove(option);

            await _specificationAttributeRepository.UpdateAsync(attribute, cancellationToken: cancellationToken);

            return ObjectMapper.Map<SpecificationAttribute, SpecificationAttributeDto>(attribute);

        }

        public async Task<Result<List<ElasticSpecificationAttribute>>> ListAsync(CancellationToken cancellationToken = default)
        {
            var response = await _elasticSearchClient.SearchAsync<ElasticSpecificationAttribute>(desc => desc
                .Query(qr => qr.MatchAll())
                .Size(1000)
            );

            if (!response.IsValidResponse)
            {
                return new List<ElasticSpecificationAttribute>();
            }

            return response.Documents.ToList();
        }

        public async Task<Result<ElasticSpecificationAttribute>> GetAsync(string attributeId, CancellationToken cancellationToken = default)
        {
            var response = await _elasticSearchClient.GetAsync<ElasticSpecificationAttribute>(attributeId,cancellationToken);

            if (!response.IsValidResponse)
            {
                return new Result<ElasticSpecificationAttribute>(new EntityNotFoundException(typeof(ElasticSpecificationAttribute), attributeId));
            }


            return response.Source!;
        }


        public async Task<Result<List<ElasticSpecificationAttributeOption>>> ListOptionsAsync(string attributeId, CancellationToken cancellationToken = default)
        {
            var response = await _elasticSearchClient.GetAsync<ElasticSpecificationAttribute>(attributeId, cancellationToken);

            if (!response.IsValidResponse)
            {
                return new Result<List<ElasticSpecificationAttributeOption>>(new EntityNotFoundException(typeof(ElasticSpecificationAttribute), attributeId));
            }

            var source = response.Source!;

            return source.Options;
        }

        public async Task<Result<ElasticSpecificationAttributeOption>> GetOptionAsync(string attributeId, string optionId, CancellationToken cancellationToken = default)
        {
            var response = await _elasticSearchClient.GetAsync<ElasticSpecificationAttribute>(attributeId, cancellationToken);

            if (!response.IsValidResponse)
            {
                return new Result<ElasticSpecificationAttributeOption>(new EntityNotFoundException(typeof(ElasticSpecificationAttribute), attributeId));
            }
            if (!response.IsValidResponse)
            {
                return new Result<ElasticSpecificationAttributeOption>(new EntityNotFoundException(typeof(ElasticSpecificationAttribute), attributeId));
            }


            var source = response.Source!;

            var option = source.Options.SingleOrDefault(x => x.Id == optionId);

            if (option == null)
            {
                return new Result<ElasticSpecificationAttributeOption>(new EntityNotFoundException(typeof(ElasticSpecificationAttributeOption), attributeId));
            }
            return option;
        }

        private async Task<Result<Unit>> ValidateSpecifcationAttribute(SpecificationAttributeModel model, string? attributeId = null, CancellationToken cancellationToken = default)
        {
            var query = await _specificationAttributeRepository.GetQueryableAsync();

            if(attributeId != null)
            {
                query = query.Where(x => x.Id != attributeId);
            }

            bool isNameExist = await query.AnyAsync(x => x.Name == model.Name);


            if (isNameExist)
            {
                return new Result<Unit>(new UserFriendlyException($"Specification attribute name : {model.Name} is already exist"));
            }

            return Unit.Value;
        }

        private async Task<Result<Unit>> ValidateSpecifcationAttributeOption(string attributeId, SpecificationAttributeOptionModel model , string? optionId = null, CancellationToken cancellationToken = default)
        {
            var query = await _specificationAttributeRepository.GetQueryableAsync();

            var  optionQuery = query.Where(x => x.Id == attributeId).SelectMany(x => x.Options);

            if(optionId != null)
            {
                optionQuery = optionQuery.Where(x => x.Id != optionId);
            }


            bool isNameExist = await optionQuery.AnyAsync(x => x.Name == model.Name);


            if (isNameExist)
            {
                return new Result<Unit>(new UserFriendlyException($"Specification attribute options name : {model.Name} is already exist"));
            }

            return Unit.Value;
        }

        private void PrepareSpecifcationAttribute(SpecificationAttribute attribute , SpecificationAttributeModel model)
        {
            attribute.Name = model.Name;
            attribute.Description = model.Description;

            if(model.Options != null)
            {
                attribute.Options = model.Options.Select(x => new SpecificationAttributeOption
                {
                    Name = x.Name
                }).ToList();
            }
        }
    }
}
