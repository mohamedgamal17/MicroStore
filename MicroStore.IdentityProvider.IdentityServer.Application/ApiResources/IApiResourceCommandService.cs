using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources
{
    public interface IApiResourceCommandService : IApplicationService
    {
        Task<UnitResult<ApiResourceDto>> CreateAsync(ApiResourceModel model, CancellationToken cancellationToken = default);

        Task<UnitResult<ApiResourceDto>> UpdateAsync(int apiResourceId ,ApiResourceModel model , CancellationToken cancellationToken = default);
        Task<UnitResult> DeleteAsync(int apiResourceId , CancellationToken cancellationToken = default);


        Task<UnitResult<ApiResourceDto>> AddApiSecret(int apiResourceId, SecretModel model, CancellationToken cancellationToken = default);

        Task<UnitResult<ApiResourceDto>> RemoveApiSecret(int apiResourceId, int secretId, CancellationToken cancellationToken = default);
    }
}
