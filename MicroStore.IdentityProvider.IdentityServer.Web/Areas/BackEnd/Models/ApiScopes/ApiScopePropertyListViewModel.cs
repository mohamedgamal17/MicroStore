using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiScopes
{
    public class ApiScopePropertyListViewModel : ListModel
    {
        public List<PropertyViewModel> Data { get; set; }
    }
}
