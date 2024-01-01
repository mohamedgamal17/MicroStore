using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Clients
{
    public interface IClientCommandService : IApplicationService
    {
        Task<Result<ClientDto>> CreateAsync(ClientModel model, CancellationToken cancellationToken= default);
        Task<Result<ClientDto>> UpdateAsync(int clientId ,ClientModel model, CancellationToken cancellationToken = default);
        Task<Result<Unit>> DeleteAsync(int clientId, CancellationToken cancellationToken = default);
        Task<Result<ClientDto>> AddClientSecret(int clientId, SecretModel model, CancellationToken cancellationToken = default);
        Task<Result<ClientDto>> DeleteClientSecret(int clientId,int secretId ,CancellationToken cancellationToken = default); 
        Task<Result<ClientDto>> AddClaim(int clinetId, ClaimModel model , CancellationToken cancellationToken = default);
        Task<Result<ClientDto>> UpdateClaim(int clientId, int claimId , ClaimModel model, CancellationToken cancellationToken = default);
        Task<Result<ClientDto>> RemoveClaim(int clientId, int claimId , CancellationToken cancellationToken = default);
        Task<Result<ClientDto>> AddProperty(int clinetId, PropertyModel model, CancellationToken cancellationToken = default);
        Task<Result<ClientDto>> UpdateProperty(int clientId, int propertyId, PropertyModel model, CancellationToken cancellationToken = default);
        Task<Result<ClientDto>> RemoveProperty(int clientId, int propertyId, CancellationToken cancellationToken = default);
    }
}
