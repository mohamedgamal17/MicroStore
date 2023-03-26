#nullable disable
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Categories
{
    public class CategoryModel
    {
        public string Id { get; set; }


        [DisplayName("Category Name")]
        public string Name { get; set; }

        [DisplayName("Category Description")]
        public string Description { get; set; }
    }

}
