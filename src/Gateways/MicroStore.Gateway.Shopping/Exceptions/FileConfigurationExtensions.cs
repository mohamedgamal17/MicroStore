using Ocelot.Configuration.File;


namespace MicroStore.Gateway.Shopping.Exceptions
{
    public static class FileConfigurationExtensions
    {
        const string SERVICES_KEY = "Services";
        public static IServiceCollection ConfigureDownstreamHostAndPortsPlaceholders(
       this IServiceCollection services,
       IConfiguration configuration)
        {
            services.PostConfigure<FileConfiguration>(fileConfiguration =>
            {

                foreach (var route in fileConfiguration.Routes)
                {
                    ConfigureRote(route, configuration);
                }
            });

            return services;
        }

        private static void ConfigureRote(FileRoute route, IConfiguration configuration)
        {
            foreach (var hostAndPort in route.DownstreamHostAndPorts)
            {
                var host = hostAndPort.Host;

                if (host.StartsWith("{") && host.EndsWith("}"))
                {
                    var placeHolder = host.TrimStart('{').TrimEnd('}');

                    var serviceApi = configuration.GetValue<string>($"{SERVICES_KEY}:{placeHolder}");

                    if(serviceApi == null)
                    {
                        throw new Exception($"Service api key {placeHolder} is not exist.");
                    }

                    var uri = new Uri(serviceApi);

                    hostAndPort.Host = uri.Host;
                    hostAndPort.Port = uri.Port;

                    if(route.DownstreamScheme != "ws")
                    {
                        route.DownstreamScheme = uri.Scheme;
                    }
                  
                }
            }
        }
    }
}
