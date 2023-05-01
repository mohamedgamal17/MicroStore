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
        Task<Result<ApiResourceDto>> AddSecret(int apiResourceId, SecretModel model, CancellationToken cancellationToken = default);
        Task<Result<ApiResourceDto>> RemoveSecret(int apiResourceId, int secretId, CancellationToken cancellationToken = default);
        Task<Result<ApiResourceDto>> AddProperty(int apiResourceId, PropertyModel model, CancellationToken cancellationToken = default);
        Task<Result<ApiResourceDto>> UpdateProperty(int apiResourceId, int propertyId, PropertyModel model, CancellationToken cancellationToken = default);
        Task<Result<ApiResourceDto>> RemoveProperty(int apiResourceId, int propertyId,  CancellationToken cancellationToken = default);
    }
}
