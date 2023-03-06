using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.Catalog.Application.Products
{
    public interface IProductCommandService : IApplicationService
    {

        Task<UnitResult<ProductDto>> CreateAsync(ProductModel model, CancellationToken cancellationToken = default);

        Task<UnitResult<ProductDto>> UpdateAsync(string id,ProductModel model , CancellationToken cancellationToken = default);

    
        Task<UnitResult<ProductDto>> AddProductImageAsync(string productId , CreateProductImageModel model , CancellationToken cancellationToken = default);

        Task<UnitResult<ProductDto>> UpdateProductImageAsync(string productId, string productImageId, UpdateProductImageModel model, CancellationToken cancellationToken = default);
        Task<UnitResult<ProductDto>> DeleteProductImageAsync(string productId, string productImageId, CancellationToken cancellationToken = default);
    }


}
