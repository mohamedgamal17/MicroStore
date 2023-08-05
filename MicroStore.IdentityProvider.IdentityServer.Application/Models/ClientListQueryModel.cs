using MicroStore.BuildingBlocks.Paging.Params;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Models
{
    public class ClientListQueryModel : PagingQueryParams
    {
        [MaxLength(200)]
        public string? ClientId { get; set; }
    }
}
