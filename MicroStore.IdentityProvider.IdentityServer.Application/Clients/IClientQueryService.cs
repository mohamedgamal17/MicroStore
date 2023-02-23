using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using Volo.Abp.Application.Services;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Clients
{
    public interface IClientQueryService : IApplicationService
    {
        Task<UnitResult<PagedResult<ClientDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default);

        Task<UnitResult<ClientDto>> GetAsync(int clientId, CancellationToken cancellationToken = default); 
    }
}
