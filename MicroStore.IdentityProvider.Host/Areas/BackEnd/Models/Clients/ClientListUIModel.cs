using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.Clients
{
    public class ClientListUIModel : BasePagedListModel
    {
        public IEnumerable<ClientDto> Data { get; set; }

    }
}
