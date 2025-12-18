using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models.ApiScopes
{
    public class ApiScopeSearchModel : ListModel
    {
        [MaxLength(200)]
        public string? Name { get; set; }
    }
}
