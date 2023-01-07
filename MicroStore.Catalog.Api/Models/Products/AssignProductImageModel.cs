using MicroStore.Catalog.Application.Abstractions.Common.Models;

namespace MicroStore.Catalog.Api.Models.Products
{
    public class AssignProductImageModel
    {
        public ImageModel Image { get; set; }
        public int DisplayOrder { get; set; }
    }
}
