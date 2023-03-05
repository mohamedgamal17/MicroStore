#nullable disable
using System.ComponentModel;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Categories
{
    public class CategoryModel
    {
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        [DisplayName("Category Description")]
        public string Description { get; set; }
    }

}
