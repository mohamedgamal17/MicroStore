using MicroStore.IdentityProvider.OAuth.Application.Models;

namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models.Clients
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
