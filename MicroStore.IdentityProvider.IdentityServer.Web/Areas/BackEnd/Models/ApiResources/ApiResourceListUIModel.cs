using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiResources
{
    public class ApiResourceListUIModel : BasePagedListModel
    {
        public IEnumerable<ApiResourceDto> Data { get; set; }
    }
}
