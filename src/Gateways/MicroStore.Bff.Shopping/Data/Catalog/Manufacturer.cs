namespace MicroStore.Bff.Shopping.Data.Catalog
{
    public class Manufacturer : AuditiedEntity<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
