using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiResources
{
    public class ApiResourceListViewModel : PagedListModel
    {
        public IEnumerable<ApiResourceDto> Data { get; set; }
    }
}
