using Duende.IdentityServer.EntityFramework.Entities;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using System.Net;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources
{
    public class ApiResourceSecretCommandHandler : RequestHandler,
       ICommandHandler<AddApiResourceSecretCommand, ApiResourceDto>,
        ICommandHandler<RemoveApiResourceSecretCommand, ApiResourceDto>
    {
        const string SharedSecret = "SharedSecret";

        private readonly IApiResourceRepository _apiResourceRepository;

        public ApiResourceSecretCommandHandler(IApiResourceRepository apiResourceRepository)
        {
            _apiResourceRepository = apiResourceRepository;
        }

        public async Task<ResponseResult<ApiResourceDto>> Handle(AddApiResourceSecretCommand request, CancellationToken cancellationToken)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == request.ApiResourceId, cancellationToken);

            if (apiResource == null)
            {
                return Failure<ApiResourceDto>(HttpStatusCode.NotFound, $"Api resource with id : {request.ApiResourceId} , is not exist");
            }

            if (apiResource.Secrets == null)
            {
                apiResource.Secrets = new List<ApiResourceSecret>();
            }

            apiResource.Secrets.Add(new ApiResourceSecret
            {
                Type = SharedSecret,
                Value = request.ResolveApiResourceSecret(),
                Description = request.Description
            });

            await _apiResourceRepository.UpdateAsync(apiResource, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource));

        }

        public async Task<ResponseResult<ApiResourceDto>> Handle(RemoveApiResourceSecretCommand request, CancellationToken cancellationToken)
        {
            var apiResource = await _apiResourceRepository.SingleOrDefaultAsync(x => x.Id == request.ApiResourceId, cancellationToken);

            if (apiResource == null)
            {
                return Failure<ApiResourceDto>(HttpStatusCode.NotFound, $"Api resource with id : {request.ApiResourceId} , is not exist");
            }

            if (apiResource.Secrets == null || !apiResource.Secrets.Any(x => x.Id == request.SecretId))
            {
                return Failure<ApiResourceDto>(HttpStatusCode.BadRequest, $"Api Secret key with id : {request.SecretId} , is not exist");
            }

            var secret = apiResource.Secrets.Single(x => x.Id == request.SecretId);

            apiResource.Secrets.Remove(secret);

            await _apiResourceRepository.UpdateAsync(apiResource, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource));

        }



    }
}
