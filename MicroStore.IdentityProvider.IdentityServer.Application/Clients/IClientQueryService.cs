using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using Volo.Abp.Application.Services;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Clients
{
    public interface IClientQueryService : IApplicationService
    {
        Task<Result<PagedResult<ClientDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default);
        Task<Result<ClientDto>> GetAsync(int clientId, CancellationToken cancellationToken = default); 
        Task<Result<List<ClientSecretDto>>> ListClientSecrets(int clientId, CancellationToken cancellationToken = default);
        Task<Result<ClientSecretDto>> GetClientSecret(int clientId, int secretId, CancellationToken cancellationToken = default);
        Task<Result<List<ClientClaimDto>>> ListClaims(int clinetId , CancellationToken cancellationToken = default);
        Task<Result<ClientClaimDto>> GetClaim(int clientId , int claimId, CancellationToken cancellationToken = default);
        Task<Result<List<ClientPropertyDto>>> ListProperties(int clientId, CancellationToken cancellationToken = default);
        Task<Result<ClientPropertyDto>> GetProperty(int clientId, int propertId, CancellationToken cancellationToken = default);

        
    }
}
