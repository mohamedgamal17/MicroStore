using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Clients
{
    public interface IClientCommandService : IApplicationService
    {
        Task<UnitResultV2<ClientDto>> CreateAsync(ClientModel model, CancellationToken cancellationToken= default);

        Task<UnitResultV2<ClientDto>> UpdateAsync(int clientId ,ClientModel model, CancellationToken cancellationToken = default);

        Task<UnitResultV2> DeleteAsync(int clientId, CancellationToken cancellationToken = default);

        Task<UnitResultV2<ClientDto>> AddClientSecret(int clientId, SecretModel model, CancellationToken cancellationToken = default);

        Task<UnitResultV2<ClientDto>> DeleteClientSecret(int clientId,int secretId ,CancellationToken cancellationToken = default);
    }
}
