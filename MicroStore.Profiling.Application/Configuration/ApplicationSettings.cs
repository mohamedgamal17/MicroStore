namespace MicroStore.Profiling.Application.Configuration
{
    public class ApplicationSettings
    {
        public SecuritySettings Security { get; set; } = new SecuritySettings();
        public ConnectionStringSettings ConnectionStrings { get; set; } = new ConnectionStringSettings();
    }


}
