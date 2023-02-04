using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using System.Net;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;
using Duende.IdentityServer.EntityFramework.Entities;
using static System.Formats.Asn1.AsnWriter;
using System.Security.Claims;
using System.Xml.Linq;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources
{
    public class ApiResourceCommandHandler : RequestHandler,
        ICommandHandler<CreateApiResourceCommand, ApiResourceDto>,
        ICommandHandler<UpdateApiResourceCommand, ApiResourceDto>,
        ICommandHandler<RemoveApiResourceCommand>
    {
        const string SharedSecret = "SharedSecret";

        private readonly IApiResourceRepository _apiResourceRepostiroy;

        public ApiResourceCommandHandler(IApiResourceRepository apiResourceRepostiroy)
        {
            _apiResourceRepostiroy = apiResourceRepostiroy;
        }

        public async Task<ResponseResult<ApiResourceDto>> Handle(CreateApiResourceCommand request, CancellationToken cancellationToken)
        {
            var apiResource = new ApiResource();

            PrepareApiResourceEntity(request, apiResource);

            await _apiResourceRepostiroy.InsertAsync(apiResource, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.Created, ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource));
        }

        public async Task<ResponseResult<ApiResourceDto>> Handle(UpdateApiResourceCommand request, CancellationToken cancellationToken)
        {
            var apiResource = await _apiResourceRepostiroy.SingleOrDefaultAsync(x => x.Id == request.ApiResourceId);

            if (apiResource == null)
            {
                return Failure<ApiResourceDto>(HttpStatusCode.NotFound, $"Api resource with id : {request.ApiResourceId} , is not exist");
            }

            PrepareApiResourceEntity(request, apiResource);

            await _apiResourceRepostiroy.UpdateApiResourceAsync(apiResource, cancellationToken);

            return Success(HttpStatusCode.OK, ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource));
        }

        public async Task<ResponseResult<Unit>> Handle(RemoveApiResourceCommand request, CancellationToken cancellationToken)
        {
            var apiResource = await _apiResourceRepostiroy.SingleOrDefaultAsync(x => x.Id == request.ApiResourceId);

            if (apiResource == null)
            {
                return Failure(HttpStatusCode.NotFound, $"Api resource with id : {request.ApiResourceId} , is not exist");
            }

            await _apiResourceRepostiroy.DeleteAsync(apiResource, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.NoContent);
        }

        private void PrepareApiResourceEntity(ApiResourceCommand request, ApiResource apiResource)
        {
            apiResource.Name = request.Name;
            apiResource.DisplayName = request.DisplayName ?? string.Empty;
            apiResource.Description = request.Description ?? string.Empty;
            apiResource.ShowInDiscoveryDocument = request.ShowInDiscoveryDocument;
            apiResource.RequireResourceIndicator = request.RequireResourceIndicator;
            apiResource.UserClaims = request.UserClaims?.Select(x => new ApiResourceClaim { Type = x }).ToList() ?? new List<ApiResourceClaim>();
            apiResource.Scopes = request.Scopes?.Select(x => new ApiResourceScope { Scope = x }).ToList() ?? new List<ApiResourceScope>();

            apiResource.Properties = request.Properties?.Select(x => new ApiResourceProperty { Key = x.Key, Value = x.Value }).ToList
                 () ?? new List<ApiResourceProperty>();
        }
    }
}
