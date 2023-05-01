using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.Clients
{
    public class ClientSecretListUIModel : BasePagedListModel
    {
        public IEnumerable<ClientSecretDto> Data { get; set; }
    }
}
