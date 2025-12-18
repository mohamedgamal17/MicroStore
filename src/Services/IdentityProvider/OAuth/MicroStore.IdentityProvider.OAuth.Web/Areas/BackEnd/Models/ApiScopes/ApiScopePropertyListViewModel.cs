using MicroStore.IdentityProvider.OAuth.Application.Dtos;

namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models.ApiScopes
{
    public class ApiScopePropertyListViewModel : ListModel
    {
        public List<PropertyViewModel> Data { get; set; }
    }
}
