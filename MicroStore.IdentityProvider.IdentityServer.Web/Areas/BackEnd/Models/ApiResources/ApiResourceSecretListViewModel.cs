using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiResources
{
    public class ApiResourceSecretListViewModel : ListModel
    {
        public List<ApiResourceSecretDto> Data { get; set; }
    }
}
