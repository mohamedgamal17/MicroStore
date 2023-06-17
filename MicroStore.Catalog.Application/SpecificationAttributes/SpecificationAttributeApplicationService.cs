using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.SpecificationAttributes;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.SpecificationAttributes
{
    public class SpecificationAttributeApplicationService : CatalogApplicationService, ISpecificationAttributeApplicationService
    {
        private readonly IRepository<SpecificationAttribute> _specificationAttributeRepository;

        private readonly ICatalogDbContext _catalogDbContext;

        public SpecificationAttributeApplicationService(IRepository<SpecificationAttribute> specificationAttributeRepository, ICatalogDbContext catalogDbContext)
        {
            _specificationAttributeRepository = specificationAttributeRepository;
            _catalogDbContext = catalogDbContext;
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

        public async Task<Result<List<SpecificationAttributeDto>>> ListAsync(CancellationToken cancellationToken = default)
        {
            var query = _catalogDbContext.SpecificationAttributes
                .AsNoTracking()
                .Include(x => x.Options)
                .ProjectTo<SpecificationAttributeDto>(MapperAccessor.Mapper.ConfigurationProvider);


            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Result<SpecificationAttributeDto>> GetAsync(string attributeId, CancellationToken cancellationToken = default)
        {
            var query = _catalogDbContext.SpecificationAttributes
                  .AsNoTracking()
                  .Include(x => x.Options)
                  .ProjectTo<SpecificationAttributeDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var attribute = await query.SingleOrDefaultAsync(x => x.Id == attributeId, cancellationToken);

            if(attribute == null)
            {
                return new Result<SpecificationAttributeDto>(new EntityNotFoundException(typeof(SpecificationAttributeOption), attributeId));
            }

            return attribute;
        }


        public async Task<Result<List<SpecificationAttributeOptionDto>>> ListOptionsAsync(string attributeId, CancellationToken cancellationToken = default)
        {
            var query = _catalogDbContext.SpecificationAttributes
                .AsNoTracking()
                .Include(x => x.Options)
                .ProjectTo<SpecificationAttributeDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var attribute = await query.SingleOrDefaultAsync(x => x.Id == attributeId, cancellationToken);

            if (attribute == null)
            {
                return new Result<List<SpecificationAttributeOptionDto>>(new EntityNotFoundException(typeof(SpecificationAttributeOption), attributeId));
            }


            return attribute.Options;
        }

        public async Task<Result<SpecificationAttributeOptionDto>> GetOptionAsync(string attributeId, string optionId, CancellationToken cancellationToken = default)
        {
            var query = _catalogDbContext.SpecificationAttributes
               .AsNoTracking()
               .Include(x => x.Options)
               .ProjectTo<SpecificationAttributeDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var attribute = await query.SingleOrDefaultAsync(x => x.Id == attributeId, cancellationToken);

            if (attribute == null)
            {
                return new Result<SpecificationAttributeOptionDto>(new EntityNotFoundException(typeof(SpecificationAttributeOption), attributeId));
            }

            var option = attribute.Options.SingleOrDefault(x => x.Id == optionId);


            if(option == null)
            {
                return new Result<SpecificationAttributeOptionDto>(new EntityNotFoundException(typeof(SpecificationAttributeOption), optionId));
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
