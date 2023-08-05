using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.Clients
{
    public class ClientSearchModel : PagedListModel
    {
        [MaxLength(200)]
        public string? ClientId { get; set; }
    }
}
