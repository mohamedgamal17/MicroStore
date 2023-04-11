using MicroStore.IdentityProvider.Identity.Application.Models;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.Users
{
    public class EditUserModel : UserModel
    {
        public string Id { get; set; }

    }
}
