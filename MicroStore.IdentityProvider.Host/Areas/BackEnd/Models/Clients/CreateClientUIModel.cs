namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.Clients
{
    public class CreateClientUIModel
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }

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
