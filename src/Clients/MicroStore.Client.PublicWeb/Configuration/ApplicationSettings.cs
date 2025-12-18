namespace MicroStore.Client.PublicWeb.Configuration
{
    public class ApplicationSettings
    {
        public SecuritySettings Security { get; set; } = new SecuritySettings();
        public S3StorageProviderSettings S3ObjectProvider { get; set; } = new S3StorageProviderSettings();
    }
}
