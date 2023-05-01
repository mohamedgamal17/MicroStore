using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.Clients
{
    public class ClientClaimListModel : BaseListModel
    {
        public List<ClientClaimDto> Data { get; set; }
    }
}
