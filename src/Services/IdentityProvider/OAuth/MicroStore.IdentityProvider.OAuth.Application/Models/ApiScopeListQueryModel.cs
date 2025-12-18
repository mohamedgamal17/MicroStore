using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.OAuth.Application.Models
{
    public class ApiScopeListQueryModel
    {
        [MaxLength(200)]
        public string? Name { get; set; }
    }
}
