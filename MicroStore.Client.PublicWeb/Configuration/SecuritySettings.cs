namespace MicroStore.Client.PublicWeb.Configuration
{
    public class SecuritySettings
    {
        public ClientSettings Client { get; set; } = new ClientSettings();
    }

    public class ClientSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Authority { get; set; }
        public List<string> Scopes { get; } = new List<string>();
    }
}
