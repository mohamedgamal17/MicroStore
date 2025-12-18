using MicroStore.IdentityProvider.OAuth.Application.Dtos;

namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models.Clients
{
    public class ClientSecretListViewModel
    {
        public IEnumerable<ClientSecretDto> Data { get; set; }
    }
}
