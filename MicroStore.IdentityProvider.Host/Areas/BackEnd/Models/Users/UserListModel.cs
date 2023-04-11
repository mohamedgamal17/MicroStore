using MicroStore.IdentityProvider.Identity.Application.Dtos;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.Users
{
    public class UserListModel : BasePagedListModel
    {
        public IEnumerable<IdentityUserDto> Data { get; set; }
    }
}
