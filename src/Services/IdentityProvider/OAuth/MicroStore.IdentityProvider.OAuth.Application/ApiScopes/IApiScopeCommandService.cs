using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.IdentityProvider.OAuth.Application.Dtos;
using MicroStore.IdentityProvider.OAuth.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.IdentityProvider.OAuth.Application.ApiScopes
{
    public interface IApiScopeCommandService : IApplicationService
    {
        Task<Result<ApiScopeDto>> CreateAsync(ApiScopeModel model, CancellationToken cancellationToken = default);
        Task<Result<ApiScopeDto>> UpdateAsync(int apiScopeId, ApiScopeModel model, CancellationToken cancellationToken = default);
        Task<Result<Unit>> DeleteAsync(int apiScopeId, CancellationToken cancellationToken = default);
        Task<Result<ApiScopeDto>> AddProperty(int apiScopeId, PropertyModel model, CancellationToken cancellationToken = default);
        Task<Result<ApiScopeDto>> UpdateProperty(int apiScopeId, int propertyId, PropertyModel model, CancellationToken cancellationToken = default);
        Task<Result<ApiScopeDto>> RemoveProperty(int apiScopeId, int propertyId, CancellationToken cancellationToken = default);

    }
}
