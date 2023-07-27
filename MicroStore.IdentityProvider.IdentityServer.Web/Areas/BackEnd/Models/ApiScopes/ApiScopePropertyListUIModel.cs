using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiScopes
{
    public class ApiScopePropertyListUIModel : BaseListModel
    {
        public List<ApiScopePropertyDto> Data { get; set; }
    }
}
