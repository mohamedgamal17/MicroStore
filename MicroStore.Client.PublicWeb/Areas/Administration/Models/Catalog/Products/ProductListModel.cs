using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products
{
    public class ProductListModel : BasePagedListModel
    {
        [BindNever]
        public List<ProductVM> Data { get; set; } = new List<ProductVM>();


  
    }
}
