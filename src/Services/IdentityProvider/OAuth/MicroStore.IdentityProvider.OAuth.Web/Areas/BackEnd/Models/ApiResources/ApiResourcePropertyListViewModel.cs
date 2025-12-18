using MicroStore.IdentityProvider.OAuth.Application.Dtos;
using MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models;

namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models.ApiResources
{
    public class ApiResourcePropertyListViewModel : ListModel
    {
        public List<PropertyViewModel> Data { get; set; }
    }
}
