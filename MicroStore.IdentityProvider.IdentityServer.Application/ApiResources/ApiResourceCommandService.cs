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
        private readonly IRepository<ApiResource> _apiResourceRepository;

        const string SharedSecret = "SharedSecret";

        public ApiResourceCommandService(IRepository<ApiResource> apiResourceRepository)
        {
            _apiResourceRepository = apiResourceRepository;
        }

        public async Task<Result<ApiResourceDto>> CreateAsync(ApiResourceModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateApiResource(model, cancellationToken : cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ApiResourceDto>(validationResult.Exception);
            }

            var apiResource = ObjectMapper.Map<ApiResourceModel, ApiResource>(model);

            apiResource =  await _apiResourceRepository.InsertAsync(apiResource, cancellationToken);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);

        }
        public async Task<Result<ApiResourceDto>> UpdateAsync(int apiResourceId, ApiResourceModel model, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == apiResourceId);

            if (apiResource == null)
            {
                return new Result<ApiResourceDto>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
            }
            var validationResult = await ValidateApiResource(model,apiResourceId , cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ApiResourceDto>(validationResult.Exception);
            }

            ObjectMapper.Map(model, apiResource);

            await _apiResourceRepository.UpdateAsync(apiResource, cancellationToken);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);

        }
        public async Task<Result<Unit>> DeleteAsync(int apiResourceId, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == apiResourceId);

            if (apiResource == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
            }

            await _apiResourceRepository.DeleteAsync(apiResource,cancellationToken);

            return Unit.Value;
        }

       public async Task<Result<ApiResourceDto>> AddSecret(int apiResourceId, SecretModel model, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == apiResourceId, cancellationToken);

            if (apiResource == null)
            {
                return new Result<ApiResourceDto>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
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

       public  async Task<Result<ApiResourceDto>> RemoveSecret(int apiResourceId, int secretId, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == apiResourceId, cancellationToken);

            if (apiResource == null)
            {
                return new Result<ApiResourceDto>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
            }

            if (apiResource.Secrets == null || !apiResource.Secrets.Any(x => x.Id == secretId))
            {
                return new Result<ApiResourceDto>(new EntityNotFoundException(typeof(ApiResourceSecret), secretId));
            }

            var secret = apiResource.Secrets.Single(x => x.Id == secretId);

            apiResource.Secrets.Remove(secret);

            await _apiResourceRepository.UpdateAsync(apiResource, cancellationToken: cancellationToken);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

        private async Task<Result<Unit>> ValidateApiResource(ApiResourceModel model, int? apiResourceId = null, CancellationToken cancellationToken =default)
        {
            var query = _apiResourceRepository.Query();

            if(apiResourceId != null)
            {
               query =  query.Where(x => x.Id != apiResourceId);
            }

            if(query.Any(x=> x.Name.ToUpper() == model.Name.ToUpper()))
            {
                return new Result<Unit>(new UserFriendlyException($"Api reosurce name : {model.Name} is already exist"));
            }


            return Unit.Value;
        }

        public async Task<Result<ApiResourceDto>> AddProperty(int apiResourceId, PropertyModel model, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == apiResourceId, cancellationToken);

            if (apiResource == null)
            {
                return new Result<ApiResourceDto>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
            }


            var validationResult = await ValidateProperty(apiResourceId, model, cancellationToken : cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ApiResourceDto>(validationResult.Exception);
            }


            if (apiResource.Properties == null) apiResource.Properties = new List<ApiResourceProperty>();

            apiResource.Properties.Add(ObjectMapper.Map<PropertyModel, ApiResourceProperty>(model));

            await _apiResourceRepository.UpdateAsync(apiResource);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

        public async Task<Result<ApiResourceDto>> UpdateProperty(int apiResourceId ,int propertyId ,PropertyModel model, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == apiResourceId, cancellationToken);

            if (apiResource == null)
            {
                return new Result<ApiResourceDto>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
            }

            if (apiResource.Properties == null ||
                !apiResource.Properties.Any(x => x.Id == propertyId))
            {
                return new Result<ApiResourceDto>(new EntityNotFoundException(typeof(ApiResourceProperty), propertyId));
            }

            var validationResult = await ValidateProperty(apiResourceId, model, propertyId, cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<ApiResourceDto>(validationResult.Exception);
            }

            var property = apiResource.Properties.SingleOrDefault(x => x.Id == propertyId);

            ObjectMapper.Map(model, property);

            await _apiResourceRepository.UpdateAsync(apiResource, cancellationToken);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

        public async Task<Result<ApiResourceDto>> RemoveProperty(int apiResourceId, int propertyId, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == apiResourceId, cancellationToken);

            if (apiResource == null)
            {
                return new Result<ApiResourceDto>(new EntityNotFoundException(typeof(ApiResource), apiResourceId));
            }

            if (apiResource.Properties == null ||
                !apiResource.Properties.Any(x => x.Id == propertyId))
            {
                return new Result<ApiResourceDto>(new EntityNotFoundException(typeof(ApiResourceProperty), propertyId));
            }

            var property = apiResource.Properties.SingleOrDefault(x => x.Id == propertyId);

            apiResource.Properties.Remove(property);

            await _apiResourceRepository.UpdateAsync(apiResource, cancellationToken);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }


        private async Task<Result<Unit>> ValidateProperty(int apiResourceId, PropertyModel model, int? propertyId = null, CancellationToken cancellationToken = default)
        {
            var apiResource = await _apiResourceRepository.SingleAsync(x => x.Id == apiResourceId, cancellationToken);

            var properties = apiResource.Properties.WhereIf(propertyId != null, (x) => x.Id != propertyId).ToList();

            var prop = properties.SingleOrDefault(x => x.Key == model.Key);
            if (properties.Any(x => x.Key.ToUpper() == model.Key.ToUpper()))
            {
                return new Result<Unit>(new UserFriendlyException($"There is already property with key {model.Key}"));
            }

            return new Result<Unit>(Unit.Value);
        }


    }
}
