namespace MicroStore.Bff.Shopping.Models.Catalog.Manufacturers
{
    public class ManufacturerModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ManufacturerModel()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }
}
