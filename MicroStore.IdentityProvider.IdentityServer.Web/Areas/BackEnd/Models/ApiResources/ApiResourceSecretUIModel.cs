using MicroStore.IdentityProvider.IdentityServer.Application.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiResources
{
    public class ApiResourceSecretUIModel
    {
        public int ApiResourceId { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class RemoveApiResourceSecretUIModel
    {
        public int ApiResourceId { get; set; }

        public int ApiResourceSecretId { get; set; }
    }
}
