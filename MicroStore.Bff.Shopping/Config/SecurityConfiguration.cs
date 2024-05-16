namespace MicroStore.Bff.Shopping.Config
{
    public class SecurityConfiguration
    {
        public JwtConfiguration Jwt { get; set; } = new JwtConfiguration();
        public JwtClientConfiguration DownStreamClient { get; set; } = new JwtClientConfiguration();
        public JwtClientConfiguration? SwaggerClient { get; set; } = new JwtClientConfiguration();
    }

    public class JwtConfiguration
    {
        public string Audience { get; set; }
        public string Authority { get; set; }
        public string TokenEndPoint { get; set; }
        public string AuthorizationEndPoint { get; set; }

        public string Secret { get; set; }
    }

    public class JwtClientConfiguration
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public Dictionary<string, string> Scopes { get; set; } = new Dictionary<string, string>();
    }
}
