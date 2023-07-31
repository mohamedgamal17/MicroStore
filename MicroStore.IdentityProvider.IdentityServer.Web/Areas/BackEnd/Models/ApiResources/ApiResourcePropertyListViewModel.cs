using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiResources
{
    public class ApiResourcePropertyListViewModel : ListModel
    {
        public List<PropertyViewModel> Data { get; set; }
    }
}
