using Duende.IdentityServer.EntityFramework.Entities;
using IdentityModel;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using System.Net;

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

        public async Task<UnitResultV2<ApiResourceDto>> CreateAsync(ApiResourceModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateApiResource(model, cancellationToken : cancellationToken);

            if (validationResult.IsFailure)
            {
                return UnitResultV2.Failure<ApiResourceDto>(validationResult.Error);
            }

            var apiResource = ObjectMapper.Map<ApiResourceModel, ApiResource>(model);

            apiResource =  await _apiResourceRepository.InsertAsync(apiResource, cancellationToken);

            return UnitResultV2.Success(ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource));

        }
        public async Task<UnitResultV2<ApiResourceDto>> UpdateAsync(int apiResourceId, ApiResourceModel model, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == apiResourceId);

            if (apiResource == null)
            {
                return UnitResultV2.Failure<ApiResourceDto>(ErrorInfo.NotFound($"Api resource with id : {apiResourceId} , is not exist"));
            }
            var validationResult = await ValidateApiResource(model,apiResourceId , cancellationToken);

            if (validationResult.IsFailure)
            {
                return UnitResultV2.Failure<ApiResourceDto>(validationResult.Error);
            }

            ObjectMapper.Map(model, apiResource);

            await _apiResourceRepository.UpdateApiResourceAsync(apiResource, cancellationToken);

            return UnitResultV2.Success(ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource));

        }
        public async Task<UnitResultV2> DeleteAsync(int apiResourceId, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == apiResourceId);

            if (apiResource == null)
            {
                return UnitResultV2.Failure<ApiResourceDto>(ErrorInfo.NotFound($"Api resource with id : {apiResourceId} , is not exist"));
            }

            await _apiResourceRepository.DeleteAsync(apiResource,cancellationToken);

            return UnitResultV2.Success();
        }

       public async Task<UnitResultV2<ApiResourceDto>> AddApiSecret(int apiResourceId, SecretModel model, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == apiResourceId, cancellationToken);

            if (apiResource == null)
            {
                return UnitResultV2.Failure<ApiResourceDto>(ErrorInfo.NotFound($"Api resource with id : {apiResourceId} , is not exist"));
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

            return UnitResultV2.Success(ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource));
        }

       public  async Task<UnitResultV2<ApiResourceDto>> RemoveApiSecret(int apiResourceId, int secretId, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == apiResourceId, cancellationToken);

            if (apiResource == null)
            {
                return UnitResultV2.Failure<ApiResourceDto>(ErrorInfo.NotFound($"Api resource with id : {apiResourceId} , is not exist"));
            }

            if (apiResource.Secrets == null || !apiResource.Secrets.Any(x => x.Id == secretId))
            {
                return UnitResultV2.Failure<ApiResourceDto>(ErrorInfo.NotFound($"Api Secret key with id : {secretId} , is not exist"));
            }

            var secret = apiResource.Secrets.Single(x => x.Id == secretId);

            apiResource.Secrets.Remove(secret);

            await _apiResourceRepository.UpdateAsync(apiResource, cancellationToken: cancellationToken);

            return UnitResultV2.Success(ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource));
        }

        private async Task<UnitResultV2> ValidateApiResource(ApiResourceModel model, int? apiResourceId = null, CancellationToken cancellationToken =default)
        {
            var query = _apiResourceRepository.Query();

            if(apiResourceId != null)
            {
                query.Where(x => x.Id != apiResourceId);
            }

            if(await _apiResourceRepository.AnyAsync(x=> x.Name == model.Name))
            {
                return UnitResultV2.Failure(ErrorInfo.BusinessLogic($"Api reosurce name : {model.Name} is already exist"));
            }


            return UnitResultV2.Success();
        }
    }
}
