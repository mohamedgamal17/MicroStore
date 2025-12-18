using MicroStore.IdentityProvider.OAuth.Application.Dtos;
using MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models;

namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models.ApiResources
{
    public class ApiResourceSecretListViewModel : ListModel
    {
        public List<ApiResourceSecretDto> Data { get; set; }
    }
}
