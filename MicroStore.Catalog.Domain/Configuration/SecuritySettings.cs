namespace MicroStore.Catalog.Domain.Configuration
{
    public class SecuritySettings
    {
        public JwtSettings Jwt { get; set; }
        public SwaggerClientSettings SwaggerClient { get; set; }
    }

    public class JwtSettings
    {
        public string Audience { get; set; }
        public string Authority { get; set; }
        public string TokenEndPoint { get; set; }
        public string AuthorizationEndPoint { get; set; }
    }

    public class SwaggerClientSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public Dictionary<string, string> Scopes { get; } = new Dictionary<string, string>();
    }
}
