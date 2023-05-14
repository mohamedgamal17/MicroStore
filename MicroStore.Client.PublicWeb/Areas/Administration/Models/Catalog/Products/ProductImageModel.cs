using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.CodeAnalysis;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products
{
    public class CreateProductImageModel
    {
        public string  ProductId { get; set; }
        public IFormFile Image { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class UpdateProductImageModel
    {
        public string  ProductId { get; set; }
        public string ProductImageId { get; set; }
        public int DisplayOrder { get; set; }


        public UpdateProductImageModel(string productImageId, string displayOrder)
        {
            ProductImageId = productImageId;

        }

        public UpdateProductImageModel()
        {

        }


    }

    public class RemoveProductImageModel
    {
        public string ProductId { get; set; }
        public string ProductImageId { get; set; }
    }


    public class ListProductImagesModel : BasePagedListModel
    {
        public List<ProductImageVM> Data { get; set; }

    }
}
