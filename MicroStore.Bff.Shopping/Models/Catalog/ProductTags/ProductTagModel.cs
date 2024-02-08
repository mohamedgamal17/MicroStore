namespace MicroStore.Bff.Shopping.Models.Catalog.ProductTags
{
    public class ProductTagModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ProductTagModel()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }
}
