using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources
{
    public interface IApiResourceCommandService : IApplicationService
    {
        Task<ResultV2<ApiResourceDto>> CreateAsync(ApiResourceModel model, CancellationToken cancellationToken = default);
        Task<ResultV2<ApiResourceDto>> UpdateAsync(int apiResourceId ,ApiResourceModel model , CancellationToken cancellationToken = default);
        Task<ResultV2<Unit>> DeleteAsync(int apiResourceId , CancellationToken cancellationToken = default);
        Task<ResultV2<ApiResourceDto>> AddApiSecret(int apiResourceId, SecretModel model, CancellationToken cancellationToken = default);
        Task<ResultV2<ApiResourceDto>> RemoveApiSecret(int apiResourceId, int secretId, CancellationToken cancellationToken = default);
    }
}
