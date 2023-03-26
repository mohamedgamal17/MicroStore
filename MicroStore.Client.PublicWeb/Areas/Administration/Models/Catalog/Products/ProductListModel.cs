using Microsoft.AspNetCore.Mvc.ModelBinding;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products
{
    public class ProductListModel : BasePagedListModel
    {
        [BindNever]
        public List<ProductVM> Data { get; set; } = new List<ProductVM>();


      
    }
}
