using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes
{
    public class ApiScopeCommandService : IdentityServiceApplicationService, IApiScopeCommandService
    {
        private readonly IApiScopeRepository _apiScopeRepository;

        public ApiScopeCommandService(IApiScopeRepository apiScopeRepository)
        {
            _apiScopeRepository = apiScopeRepository;
        }

        public async Task<ResultV2<ApiScopeDto>> CreateAsync(ApiScopeModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateApiScope(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return  new ResultV2<ApiScopeDto>(validationResult.Exception);
            }

            var apiScope = ObjectMapper.Map<ApiScopeModel, ApiScope>(model);

            apiScope = await _apiScopeRepository.InsertAsync(apiScope);

            return ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope);
        }

        public async Task<ResultV2<ApiScopeDto>> UpdateAsync(int apiScopeId, ApiScopeModel model, CancellationToken cancellationToken = default)
        {
            var apiScope = await _apiScopeRepository.SingleOrDefaultAsync(x => x.Id == apiScopeId, cancellationToken);

            if (apiScope == null)
            {
                return new ResultV2<ApiScopeDto>(new EntityNotFoundException(typeof(ApiScope), apiScopeId));
            }

            var validationResult = await ValidateApiScope(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new ResultV2<ApiScopeDto>(validationResult.Exception);
            }

            apiScope = ObjectMapper.Map(model, apiScope);

            await _apiScopeRepository.UpdateApiScopeAsync(apiScope,cancellationToken);

            return ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope);

        }

        public async Task<ResultV2<Unit>> DeleteAsync(int apiScopeId, CancellationToken cancellationToken = default)
        {
            var apiScope = await _apiScopeRepository.SingleOrDefaultAsync(x => x.Id == apiScopeId, cancellationToken);

            if (apiScope == null)
            {
                return new ResultV2<Unit>(new EntityNotFoundException(typeof(ApiScope), apiScopeId));
            }

            await _apiScopeRepository.DeleteAsync(apiScope, cancellationToken);

            return Unit.Value;
        }


        private async Task<ResultV2<Unit>> ValidateApiScope(ApiScopeModel model,int? apiScopeId = null ,CancellationToken cancellationToken = default)
        {
            var query = _apiScopeRepository.Query();

            if(apiScopeId != null)
            {
               query =  query.Where(x => x.Id != apiScopeId);
            }

            if(await query.AnyAsync(x=> x.Name == model.Name))
            {
                return new ResultV2<Unit>(new BusinessException($"Api scope with name : {model.Name} is already exist"));
            }

            return Unit.Value;
        }
    }
}
