using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.Catalog.Application.Products
{
    public interface IProductCommandService : IApplicationService
    {

        Task<ResultV2<ProductDto>> CreateAsync(ProductModel model, CancellationToken cancellationToken = default);

        Task<ResultV2<ProductDto>> UpdateAsync(string id,ProductModel model , CancellationToken cancellationToken = default);
    
        Task<ResultV2<ProductDto>> AddProductImageAsync(string productId , CreateProductImageModel model , CancellationToken cancellationToken = default);

        Task<ResultV2<ProductDto>> UpdateProductImageAsync(string productId, string productImageId, UpdateProductImageModel model, CancellationToken cancellationToken = default);
        Task<ResultV2<ProductDto>> DeleteProductImageAsync(string productId, string productImageId, CancellationToken cancellationToken = default);
    }


}
