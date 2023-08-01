using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.IdentityProvider.Identity.Domain.Shared.Models
{
    public class UserListQueryModel : PagingQueryParams
    {
        public string? UserName { get; set; }
        public string? Role { get; set; }

    }
}
