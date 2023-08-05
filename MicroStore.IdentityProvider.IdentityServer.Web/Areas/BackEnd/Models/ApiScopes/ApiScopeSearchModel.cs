using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiScopes
{
    public class ApiScopeSearchModel : ListModel
    {
        [MaxLength(200)]
        public string? Name { get; set; }
    }
}
