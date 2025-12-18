namespace MicroStore.IdentityProvider.Identity.Web.Areas.BackEnd.Models.Users
{
    public class UserSearchModel : PagedListModel
    {
        public string? UserName { get; set; }
        public string? Role { get; set; }
    }
}
