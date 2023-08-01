using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;

namespace MicroStore.IdentityProvider.Identity.Web.Areas.BackEnd.Models.Users
{
    public class UserListViewModel : PagedListModel
    {
        public IEnumerable<IdentityUserDto> Data { get; set; }
    }
}
