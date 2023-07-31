using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiResources
{
    public class ApiResourceListUIModel : PagedListModel
    {
        public IEnumerable<ApiResourceDto> Data { get; set; }
    }
}
