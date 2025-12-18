using MicroStore.IdentityProvider.OAuth.Application.Dtos;
using MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models;

namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models.ApiResources
{
    public class ApiResourceListViewModel : PagedListModel
    {
        public IEnumerable<ApiResourceDto> Data { get; set; }
    }
}
