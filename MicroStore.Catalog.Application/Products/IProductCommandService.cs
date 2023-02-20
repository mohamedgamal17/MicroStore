using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.Catalog.Application.Products
{
    public interface IProductCommandService : IApplicationService
    {

        Task<UnitResultV2<ProductDto>> CreateAsync(ProductModel model, CancellationToken cancellationToken = default);

        Task<UnitResultV2<ProductDto>> UpdateAsync(string id,ProductModel model , CancellationToken cancellationToken = default);

    
    }


}
