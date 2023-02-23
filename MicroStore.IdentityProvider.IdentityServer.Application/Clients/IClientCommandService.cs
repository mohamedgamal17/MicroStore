using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Clients
{
    public interface IClientCommandService : IApplicationService
    {
        Task<UnitResult<ClientDto>> CreateAsync(ClientModel model, CancellationToken cancellationToken= default);

        Task<UnitResult<ClientDto>> UpdateAsync(int clientId ,ClientModel model, CancellationToken cancellationToken = default);

        Task<UnitResult> DeleteAsync(int clientId, CancellationToken cancellationToken = default);

        Task<UnitResult<ClientDto>> AddClientSecret(int clientId, SecretModel model, CancellationToken cancellationToken = default);

        Task<UnitResult<ClientDto>> DeleteClientSecret(int clientId,int secretId ,CancellationToken cancellationToken = default);
    }
}
