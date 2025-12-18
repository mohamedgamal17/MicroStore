using MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models.ApiResources
{
    public class ApiResourceSearchModel : PagedListModel
    {
        [MaxLength(200)]
        public string? Name { get; set; }
    }
}
