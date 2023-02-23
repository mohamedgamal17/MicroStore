using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes
{
    public class ApiScopeCommandService : IdentityServiceApplicationService, IApiScopeCommandService
    {
        private readonly IApiScopeRepository _apiScopeRepository;

        public ApiScopeCommandService(IApiScopeRepository apiScopeRepository)
        {
            _apiScopeRepository = apiScopeRepository;
        }

        public async Task<UnitResult<ApiScopeDto>> CreateAsync(ApiScopeModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateApiScope(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return UnitResult.Failure<ApiScopeDto>(validationResult.Error);
            }

            var apiScope = ObjectMapper.Map<ApiScopeModel, ApiScope>(model);

            apiScope = await _apiScopeRepository.InsertAsync(apiScope);

            return UnitResult.Success(ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope));
        }

        public async Task<UnitResult<ApiScopeDto>> UpdateAsync(int apiScopeId, ApiScopeModel model, CancellationToken cancellationToken = default)
        {
            var apiScope = await _apiScopeRepository.SingleOrDefaultAsync(x => x.Id == apiScopeId, cancellationToken);

            if (apiScope == null)
            {
                return UnitResult.Failure<ApiScopeDto>(ErrorInfo.NotFound( $"Api Scope with id : {apiScopeId} is not exist"));
            }

            var validationResult = await ValidateApiScope(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return UnitResult.Failure<ApiScopeDto>(validationResult.Error);
            }

            apiScope = ObjectMapper.Map(model, apiScope);

            await _apiScopeRepository.UpdateApiScopeAsync(apiScope,cancellationToken);

            return UnitResult.Success(ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope));

        }

        public async Task<UnitResult> DeleteAsync(int apiScopeId, CancellationToken cancellationToken = default)
        {
            var apiScope = await _apiScopeRepository.SingleOrDefaultAsync(x => x.Id == apiScopeId, cancellationToken);

            if (apiScope == null)
            {
                return UnitResult.Failure(ErrorInfo.NotFound($"Api Scope with id : {apiScopeId} is not exist"));
            }

            await _apiScopeRepository.DeleteAsync(apiScope, cancellationToken);

            return UnitResult.Success();
        }


        private async Task<UnitResult> ValidateApiScope(ApiScopeModel model,int? apiScopeId = null ,CancellationToken cancellationToken = default)
        {
            var query = _apiScopeRepository.Query();

            if(apiScopeId != null)
            {
               query =  query.Where(x => x.Id != apiScopeId);
            }

            if(await query.AnyAsync(x=> x.Name == model.Name))
            {
                return UnitResult.Failure(ErrorInfo.BusinessLogic($"Api scope with name : {model.Name} is already exist"));
            }

            return UnitResult.Success();
        }
    }
}
