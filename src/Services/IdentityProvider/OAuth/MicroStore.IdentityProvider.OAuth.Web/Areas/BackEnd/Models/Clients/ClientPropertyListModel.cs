using MicroStore.IdentityProvider.OAuth.Application.Dtos;
using MicroStore.IdentityProvider.OAuth.Application.Models;
using MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models;

namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models.Clients
{
    public class ClientPropertyListModel : ListModel
    {
        public List<PropertyViewModel> Data { get; set; }
    }
}
