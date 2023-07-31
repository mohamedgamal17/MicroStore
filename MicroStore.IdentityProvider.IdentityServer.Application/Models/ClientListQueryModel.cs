using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Models
{
    public class ClientListQueryModel : PagingQueryParams
    {
        public string? ClientId { get; set; }
    }
}
