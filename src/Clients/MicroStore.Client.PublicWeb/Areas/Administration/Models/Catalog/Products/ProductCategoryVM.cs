using System.ComponentModel;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products
{
    public class ProductCategoryVM
    {
        [DisplayName("Product Category Id")]
        public string Id { get; set; }

        [DisplayName("Category Id")]
        public string CategoryId { get; set; }

        [DisplayName("Category Name")]
        public string Name { get; set; }
    }
}
