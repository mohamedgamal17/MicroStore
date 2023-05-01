using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes
{
    public class ApiScopeCommandService : IdentityServiceApplicationService, IApiScopeCommandService
    {
        private readonly IRepository<ApiScope> _apiScopeRepository;

        public ApiScopeCommandService(IRepository<ApiScope> apiScopeRepository)
        {
            _apiScopeRepository = apiScopeRepository;
        }

        public async Task<Result<ApiScopeDto>> CreateAsync(ApiScopeModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateApiScope(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return  new Result<ApiScopeDto>(validationResult.Exception);
            }

            var apiScope = ObjectMapper.Map<ApiScopeModel, ApiScope>(model);

            apiScope = await _apiScopeRepository.InsertAsync(apiScope);

            return ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope);
        }

        public async Task<Result<ApiScopeDto>> UpdateAsync(int apiScopeId, ApiScopeModel model, CancellationToken cancellationToken = default)
        {
            var apiScope = await _apiScopeRepository.SingleOrDefaultAsync(x => x.Id == apiScopeId, cancellationToken);

            if (apiScope == null)
            {
                return new Result<ApiScopeDto>(new EntityNotFoundException(typeof(ApiScope), apiScopeId));
            }

            var validationResult = await ValidateApiScope(model, apiScopeId ,cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ApiScopeDto>(validationResult.Exception);
            }

            apiScope = ObjectMapper.Map(model, apiScope);

            await _apiScopeRepository.UpdateAsync(apiScope,cancellationToken);

            return ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope);

        }

        public async Task<Result<Unit>> DeleteAsync(int apiScopeId, CancellationToken cancellationToken = default)
        {
            var apiScope = await _apiScopeRepository.SingleOrDefaultAsync(x => x.Id == apiScopeId, cancellationToken);

            if (apiScope == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(ApiScope), apiScopeId));
            }

            await _apiScopeRepository.DeleteAsync(apiScope, cancellationToken);

            return Unit.Value;
        }


        private async Task<Result<Unit>> ValidateApiScope(ApiScopeModel model,int? apiScopeId = null ,CancellationToken cancellationToken = default)
        {
            var query = _apiScopeRepository.Query();

            if(apiScopeId != null)
            {
               query =  query.Where(x => x.Id != apiScopeId);
            }

            if(await query.AnyAsync(x=> x.Name.ToUpper() == model.Name.ToUpper()))
            {
                return new Result<Unit>(new UserFriendlyException($"Api scope with name : {model.Name} is already exist"));
            }

            return Unit.Value;
        }

        public async Task<Result<ApiScopeDto>> AddProperty(int apiScopeId,PropertyModel model ,CancellationToken cancellationToken = default)
        {
            var apiScope = await _apiScopeRepository.SingleOrDefaultAsync(x => x.Id == apiScopeId, cancellationToken);

            if(apiScope == null)
            {
                return new Result<ApiScopeDto>(new EntityNotFoundException(typeof(ApiScope), apiScopeId));
            }

            var validationResult = await ValidateProperty(apiScopeId, model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ApiScopeDto>(validationResult.Exception);
            }

            if (apiScope.Properties == null) apiScope.Properties = new List<ApiScopeProperty>();

            apiScope.Properties.Add(ObjectMapper.Map<PropertyModel,ApiScopeProperty>(model));

            await _apiScopeRepository.UpdateAsync(apiScope);

            return ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope);
        }

        public async Task<Result<ApiScopeDto>> UpdateProperty(int apiScopeId, int propertyId, PropertyModel model, CancellationToken cancellationToken = default)
        {
            var apiScope = await _apiScopeRepository.SingleOrDefaultAsync(x => x.Id == apiScopeId, cancellationToken);

            if (apiScope == null)
            {
                return new Result<ApiScopeDto>(new EntityNotFoundException(typeof(ApiScope), apiScopeId));
            }

            if(apiScope.Properties == null ||
                !apiScope.Properties.Any(x=> x.Id == propertyId))
            {
                return new Result<ApiScopeDto>(new EntityNotFoundException(typeof(ApiScopeProperty), propertyId));
            }

            var validationResult = await ValidateProperty(apiScopeId, model, propertyId, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ApiScopeDto>(validationResult.Exception);
            }

            var property = apiScope.Properties.SingleOrDefault(x => x.Id == propertyId);

            ObjectMapper.Map(model, property);

            await _apiScopeRepository.UpdateAsync(apiScope, cancellationToken);

            return ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope);

        }

        public async Task<Result<ApiScopeDto>> RemoveProperty(int apiScopeId, int propertyId, CancellationToken cancellationToken = default)
        {
            var apiScope = await _apiScopeRepository.SingleOrDefaultAsync(x => x.Id == apiScopeId, cancellationToken);

            if (apiScope == null)
            {
                return new Result<ApiScopeDto>(new EntityNotFoundException(typeof(ApiScope), apiScopeId));
            }

            if (apiScope.Properties == null ||
                !apiScope.Properties.Any(x => x.Id == propertyId))
            {
                return new Result<ApiScopeDto>(new EntityNotFoundException(typeof(ApiScopeProperty), propertyId));
            }

            var property = apiScope.Properties.SingleOrDefault(x => x.Id == propertyId);

            apiScope.Properties.Remove(property);

            await _apiScopeRepository.UpdateAsync(apiScope, cancellationToken);

            return ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope);
        }

        private async Task<Result<Unit>> ValidateProperty(int apiScopeId, PropertyModel model, int? propertyId = null, CancellationToken cancellationToken = default)
        {
            var apiScope = await _apiScopeRepository.SingleAsync(x => x.Id == apiScopeId, cancellationToken);

            var properties = apiScope.Properties.WhereIf(propertyId != null, (x) => x.Id != propertyId).ToList();

            if (properties.Any(x => x.Key.ToUpper() == model.Key.ToUpper()))
            {
                return new Result<Unit>(new UserFriendlyException($"There is already property with key {model.Key}"));
            }

            return new Result<Unit>(Unit.Value);
        }
    }
}
