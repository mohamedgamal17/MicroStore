using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiResources
{
    public class ApiResourceSearchModel : PagedListModel
    {
        [MaxLength(200)]
        public string? Name { get; set; }
    }
}
