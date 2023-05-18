using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.Products;
using Volo.Abp.Application.Services;
namespace MicroStore.Catalog.Application.Products
{
    public interface IProductCommandService : IApplicationService
    {

        Task<Result<ProductDto>> CreateAsync(ProductModel model, CancellationToken cancellationToken = default);

        Task<Result<ProductDto>> UpdateAsync(string id,ProductModel model , CancellationToken cancellationToken = default);
    
        Task<Result<ProductDto>> AddProductImageAsync(string productId , CreateProductImageModel model , CancellationToken cancellationToken = default);

        Task<Result<ProductDto>> UpdateProductImageAsync(string productId, string productImageId, UpdateProductImageModel model, CancellationToken cancellationToken = default);
        Task<Result<ProductDto>> DeleteProductImageAsync(string productId, string productImageId, CancellationToken cancellationToken = default);
    }


}
