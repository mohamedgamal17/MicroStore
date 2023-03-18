using Duende.IdentityServer.EntityFramework.Entities;
using IdentityModel;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources
{
    public class ApiResourceCommandService : IdentityServiceApplicationService, IApiResourceCommandService
    {
        private readonly IApiResourceRepository _apiResourceRepository;

        const string SharedSecret = "SharedSecret";

        public ApiResourceCommandService(IApiResourceRepository apiResourceRepository)
        {
            _apiResourceRepository = apiResourceRepository;
        }

        public async Task<ResultV2<ApiResourceDto>> CreateAsync(ApiResourceModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateApiResource(model, cancellationToken : cancellationToken);

            if (validationResult.IsFailure)
            {
                return new ResultV2<ApiResourceDto>(validationResult.Exception);
            }

            var apiResource = ObjectMapper.Map<ApiResourceModel, ApiResource>(model);

            apiResource =  await _apiResourceRepository.InsertAsync(apiResource, cancellationToken);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);

        }
        public async Task<ResultV2<ApiResourceDto>> UpdateAsync(int apiResourceId, ApiResourceModel model, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == apiResourceId);

            if (apiResource == null)
            {
                return new ResultV2<ApiResourceDto>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
            }
            var validationResult = await ValidateApiResource(model,apiResourceId , cancellationToken);

            if (validationResult.IsFailure)
            {
                return new ResultV2<ApiResourceDto>(validationResult.Exception);
            }

            ObjectMapper.Map(model, apiResource);

            await _apiResourceRepository.UpdateApiResourceAsync(apiResource, cancellationToken);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);

        }
        public async Task<ResultV2<Unit>> DeleteAsync(int apiResourceId, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == apiResourceId);

            if (apiResource == null)
            {
                return new ResultV2<Unit>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
            }

            await _apiResourceRepository.DeleteAsync(apiResource,cancellationToken);

            return Unit.Value;
        }

       public async Task<ResultV2<ApiResourceDto>> AddApiSecret(int apiResourceId, SecretModel model, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == apiResourceId, cancellationToken);

            if (apiResource == null)
            {
                return new ResultV2<ApiResourceDto>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
            }

            if (apiResource.Secrets == null)
            {
                apiResource.Secrets = new List<ApiResourceSecret>();
            }

            apiResource.Secrets.Add(new ApiResourceSecret
            {
                Type = SharedSecret,
                Value = model.Value.ToSha512(),
                Description = model.Description
            });

            await _apiResourceRepository.UpdateAsync(apiResource, cancellationToken: cancellationToken);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

       public  async Task<ResultV2<ApiResourceDto>> RemoveApiSecret(int apiResourceId, int secretId, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == apiResourceId, cancellationToken);

            if (apiResource == null)
            {
                return new ResultV2<ApiResourceDto>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
            }

            if (apiResource.Secrets == null || !apiResource.Secrets.Any(x => x.Id == secretId))
            {
                return new ResultV2<ApiResourceDto>(new EntityNotFoundException(typeof(ApiResourceSecret), secretId));
            }

            var secret = apiResource.Secrets.Single(x => x.Id == secretId);

            apiResource.Secrets.Remove(secret);

            await _apiResourceRepository.UpdateAsync(apiResource, cancellationToken: cancellationToken);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

        private async Task<ResultV2<Unit>> ValidateApiResource(ApiResourceModel model, int? apiResourceId = null, CancellationToken cancellationToken =default)
        {
            var query = _apiResourceRepository.Query();

            if(apiResourceId != null)
            {
                query.Where(x => x.Id != apiResourceId);
            }

            if(await _apiResourceRepository.AnyAsync(x=> x.Name == model.Name))
            {
                return new ResultV2<Unit>(new BusinessException($"Api reosurce name : {model.Name} is already exist"));
            }


            return Unit.Value;
        }
    }
}
