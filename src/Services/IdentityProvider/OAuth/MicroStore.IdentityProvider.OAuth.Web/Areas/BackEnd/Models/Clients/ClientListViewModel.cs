using MicroStore.IdentityProvider.OAuth.Application.Dtos;
using MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models;
namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models.Clients
{
    public class ClientListViewModel : PagedListModel
    {
        public IEnumerable<ClientDto> Data { get; set; }
    }
}
