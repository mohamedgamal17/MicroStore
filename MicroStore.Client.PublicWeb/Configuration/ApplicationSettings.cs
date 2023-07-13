namespace MicroStore.Client.PublicWeb.Configuration
{
    public class ApplicationSettings
    {
        public SecuritySettings Security { get; set; } = new SecuritySettings();
        public MinioSettings Minio { get; set; } = new MinioSettings();
    }
}
