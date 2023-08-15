namespace MicroStore.Catalog.Domain.Entities
{
    public class ImageVector
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ImageId { get; set; }
        public List<float> Features { get; set; }
    }
}
