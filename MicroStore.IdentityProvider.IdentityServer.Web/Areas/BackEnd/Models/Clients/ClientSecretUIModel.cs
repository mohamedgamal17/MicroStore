using MicroStore.IdentityProvider.IdentityServer.Application.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.Clients
{
    public class ClientSecretUIModel : SecretModel
    {
        public int ClientId { get; set; }
    }


    public class RemoveClientSecretUIModel
    {
        public int ClientId { get; set; }

        public int SecretId { get; set; }
    }
}
