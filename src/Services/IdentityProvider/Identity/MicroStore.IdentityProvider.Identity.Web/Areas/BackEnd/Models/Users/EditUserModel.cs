using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.Identity.Web.Areas.BackEnd.Models.Users
{
    public class EditUserModel : UserModel
    {
        public string Id { get; set; }

    }
}
