using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.ApiScopes
{
    public class ApiScopePropertyListUIModel : BaseListModel
    {
        public List<ApiScopePropertyDto> Data { get; set; }
    }
}
