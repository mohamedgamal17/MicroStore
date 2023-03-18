using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources
{
    public interface IApiResourceCommandService : IApplicationService
    {
        Task<Result<ApiResourceDto>> CreateAsync(ApiResourceModel model, CancellationToken cancellationToken = default);
        Task<Result<ApiResourceDto>> UpdateAsync(int apiResourceId ,ApiResourceModel model , CancellationToken cancellationToken = default);
        Task<Result<Unit>> DeleteAsync(int apiResourceId , CancellationToken cancellationToken = default);
        Task<Result<ApiResourceDto>> AddApiSecret(int apiResourceId, SecretModel model, CancellationToken cancellationToken = default);
        Task<Result<ApiResourceDto>> RemoveApiSecret(int apiResourceId, int secretId, CancellationToken cancellationToken = default);
    }
}
