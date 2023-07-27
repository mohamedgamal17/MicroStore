using Microsoft.AspNetCore.Mvc.ModelBinding;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.Clients
{
    public class ClientListUIModel : BasePagedListModel
    {
        [BindNever]
        public IEnumerable<ClientDto> Data { get; set; }

    }
}
