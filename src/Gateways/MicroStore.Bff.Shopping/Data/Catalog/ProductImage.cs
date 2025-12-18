namespace MicroStore.Bff.Shopping.Data.Catalog
{
    public class ProductImage : Entity<string>
    {
        public string Image { get; set; }
        public int DisplayOrder { get; set; }
    }
}
