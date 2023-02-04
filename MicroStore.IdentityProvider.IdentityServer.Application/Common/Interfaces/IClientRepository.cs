using Duende.IdentityServer.EntityFramework.Entities;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task UpdateClinetAsync(Client client, CancellationToken cancellationToken = default);
    }
}
