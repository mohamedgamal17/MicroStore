using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.Clients
{
    public class CreateClientModel
    {
        [MaxLength(200)]
        [Required]
        public string ClientId { get; set; }

        [MaxLength(200)]
        [Required]
        public string ClientName { get; set; }

        [Required]
        public ClientType Type { get; set; }
    }

    public enum ClientType
    {
        Web,
        Spa,
        Native,
        Machine,
        Device
    }
}
