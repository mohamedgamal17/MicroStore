using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using System.ComponentModel;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Categories
{
    public class CategoryListModel : BaseListModel
    {
        public List<CategoryVM> Data { get; set; }
    }
}
