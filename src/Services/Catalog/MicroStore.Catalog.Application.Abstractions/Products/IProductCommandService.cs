using MicroStore.BuildingBlocks.Utils.Results;
using Volo.Abp.Application.Services;
namespace MicroStore.Catalog.Application.Abstractions.Products
{
    public interface IProductCommandService : IApplicationService
    {

        Task<Result<ProductDto>> CreateAsync(ProductModel model, CancellationToken cancellationToken = default);

        Task<Result<ProductDto>> UpdateAsync(string id, ProductModel model, CancellationToken cancellationToken = default);

        Task<Result<ProductImageDto>> AddProductImageAsync(string productId, ProductImageModel model, CancellationToken cancellationToken = default);

        Task<Result<ProductImageDto>> UpdateProductImageAsync(string productId, string productImageId, ProductImageModel model, CancellationToken cancellationToken = default);
        Task<Result<Unit>> DeleteProductImageAsync(string productId, string productImageId, CancellationToken cancellationToken = default);

        Task<Result<ProductDto>> CreateProductAttributeSpecificationAsync(string productId, ProductSpecificationAttributeModel model, CancellationToken cancellationToken = default);

        Task<Result<ProductDto>> RemoveProductAttributeSpecificationAsync(string productId, string attributeId, CancellationToken cancellationToken = default);
    }


}
