using MicroStore.BuildingBlocks.Utils.Paging.Params;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.OAuth.Application.Models
{
    public class ClientListQueryModel : PagingQueryParams
    {
        [MaxLength(200)]
        public string? ClientId { get; set; }
    }
}
