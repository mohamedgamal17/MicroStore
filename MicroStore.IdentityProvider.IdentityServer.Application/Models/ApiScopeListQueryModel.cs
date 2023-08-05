using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Models
{
    public class ApiScopeListQueryModel
    {
        [MaxLength(200)]
        public string? Name { get; set; }
    }
}
