using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.ApiResources
{
    public class ApiResourceListUIModel : BasePagedListModel
    {
        public IEnumerable<ApiResourceDto> Data { get; set; }
    }
}
