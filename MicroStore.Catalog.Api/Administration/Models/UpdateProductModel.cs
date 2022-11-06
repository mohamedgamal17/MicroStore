#nullable disable
namespace MicroStore.Catalog.Api.Administration.Models
{
    public class UpdateProductModel
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
    }
}
