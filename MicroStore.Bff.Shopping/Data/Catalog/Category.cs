namespace MicroStore.Bff.Shopping.Data.Catalog
{
    public class Category : AuditiedEntity<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
