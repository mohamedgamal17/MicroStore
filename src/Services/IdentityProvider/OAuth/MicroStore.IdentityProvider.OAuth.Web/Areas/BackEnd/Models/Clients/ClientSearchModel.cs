using MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models.Clients
{
    public class ClientSearchModel : PagedListModel
    {
        [MaxLength(200)]
        public string? ClientId { get; set; }
    }
}
