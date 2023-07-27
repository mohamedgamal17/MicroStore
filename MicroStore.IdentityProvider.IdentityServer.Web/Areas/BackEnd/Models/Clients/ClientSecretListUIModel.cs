using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.Clients
{
    public class ClientSecretListUIModel : BasePagedListModel
    {
        public IEnumerable<ClientSecretDto> Data { get; set; }
    }
}
