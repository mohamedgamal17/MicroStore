using MicroStore.IdentityProvider.IdentityServer.Application.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiResources
{
    public class CreateApiResourceSecretModel

    {
        public int ApiResourceId { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class RemoveApiResourceSecretModel
    {
        public int ApiResourceId { get; set; }

        public int SecretId { get; set; }
    }
}
