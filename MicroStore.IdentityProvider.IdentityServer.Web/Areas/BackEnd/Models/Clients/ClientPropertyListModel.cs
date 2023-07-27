using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.Clients
{
    public class ClientPropertyListModel : BaseListModel
    {
        public List<ClientPropertyDto> Data { get; set; }
    }
}
