using MicroStore.IdentityProvider.OAuth.Application.Dtos;

namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models.ApiScopes
{
    public class ApiScopeListViewModel : ListModel
    {
        public List<ApiScopeDto> Data { get; set; }

    }
}
