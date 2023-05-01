using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.ApiResources
{
    public class ApiResourcePropertyListUIModel : BaseListModel
    {
        public List<ApiResourcePropertyDto> Data { get; set; }
    }
}
