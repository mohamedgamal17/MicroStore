namespace MicroStore.Bff.Shopping.Data.Catalog
{
    public class ProductCategory : Entity<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
