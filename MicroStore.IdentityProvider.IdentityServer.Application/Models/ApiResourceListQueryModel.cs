using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Models
{
    public class ApiResourceListQueryModel : PagingQueryParams
    {
        public string? Name { get; set; }

    }
}
