namespace MicroStore.Profiling.Application.Configuration
{
    public class SecuritySettings
    {
        public JwtSettings Jwt { get; set; } = new JwtSettings();
        public SwaggerClientSettings SwaggerClient { get; set; } = new SwaggerClientSettings();
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
