using System.ComponentModel;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products
{
    public class ProductImageVM
    {
        [DisplayName("Product Image Id")]
        public string  Id { get; set; }

        [DisplayName("Product Image")]
        public string ImagePath { get; set; }

        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}
