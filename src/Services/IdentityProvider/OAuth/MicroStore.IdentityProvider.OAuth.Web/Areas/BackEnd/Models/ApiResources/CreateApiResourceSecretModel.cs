using MicroStore.IdentityProvider.OAuth.Application.Models;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models.ApiResources
{
    public class CreateApiResourceSecretModel
    {
        public int ApiResourceId { get; set; }

        [MaxLength(200)]
        public string Value { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }
    }

    public class RemoveApiResourceSecretModel
    {
        public int ApiResourceId { get; set; }

        public int SecretId { get; set; }
    }
}
