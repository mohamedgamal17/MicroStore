using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.Clients
{
    public class ClientListViewModel : PagedListModel
    {
        public IEnumerable<ClientDto> Data { get; set; }
    }
}
