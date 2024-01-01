using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Clients
{
    public interface IClientQueryService : IApplicationService
    {
        Task<Result<PagedResult<ClientDto>>> ListAsync(ClientListQueryModel queryParams, CancellationToken cancellationToken = default);
        Task<Result<ClientDto>> GetAsync(int clientId, CancellationToken cancellationToken = default); 
        Task<Result<List<ClientSecretDto>>> ListClientSecrets(int clientId, CancellationToken cancellationToken = default);
        Task<Result<ClientSecretDto>> GetClientSecret(int clientId, int secretId, CancellationToken cancellationToken = default);
        Task<Result<List<ClientClaimDto>>> ListClaims(int clinetId , CancellationToken cancellationToken = default);
        Task<Result<ClientClaimDto>> GetClaim(int clientId , int claimId, CancellationToken cancellationToken = default);
        Task<Result<List<ClientPropertyDto>>> ListProperties(int clientId, CancellationToken cancellationToken = default);
        Task<Result<ClientPropertyDto>> GetProperty(int clientId, int propertId, CancellationToken cancellationToken = default);

        
    }
}
