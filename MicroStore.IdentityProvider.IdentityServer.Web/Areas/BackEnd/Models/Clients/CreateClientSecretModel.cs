using MicroStore.IdentityProvider.IdentityServer.Application.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.Clients
{
    public class CreateClientSecretModel : SecretModel
    {
        public int ClientId { get; set; }
    }


    public class RemoveClientSecretModel
    {
        public int ClientId { get; set; }

        public int SecretId { get; set; }
    }
}
