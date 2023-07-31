using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources
{
    public interface IApiResourceQueryService : IApplicationService
    {
        Task<Result<PagedResult<ApiResourceDto>>> ListAsync(ApiResourceListQueryModel queryParams, CancellationToken cancellationToken = default);
        Task<Result<ApiResourceDto>> GetAsync(int apiResourceId , CancellationToken cancellationToken = default);
        Task<Result<List<ApiResourceSecretDto>>> ListApiResourceSecrets(int apiResourceId, CancellationToken cancellationToken = default);
        Task<Result<ApiResourceSecretDto>> GetApiResourceSecret(int  apiResourceId ,int secretId ,CancellationToken cancellationToken = default);
        Task<Result<List<ApiResourcePropertyDto>>> ListProperties(int apiResourceId, CancellationToken cancellationToken = default);
        Task<Result<ApiResourcePropertyDto>> GetApiResourceProperty(int apiResourceId , int propertyId , CancellationToken cancellationToken = default);    
    }
}
