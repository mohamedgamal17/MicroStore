using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.ApiScopes
{
    public class ApiScopeListUIModel  : BaseListModel
    {
        public List<ApiScopeDto> Data { get; set; }

    }
}
