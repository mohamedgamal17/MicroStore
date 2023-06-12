using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.ProductTags;
using Volo.Abp.Application.Services;

namespace MicroStore.Catalog.Application.ProductTags
{
    public interface IProductTagApplicationService  : IApplicationService
    {
        Task<Result<ProductTagDto>> CreateAsync(ProductTagModel model , CancellationToken cancellationToken = default);
        Task<Result<ProductTagDto>> UpdateAsync(string productTagId, ProductTagModel productTag , CancellationToken cancellationToken = default);
        Task<Result<Unit>> RemoveAsync(string productTagId, CancellationToken cancellationToken = default);
        Task<Result<List<ProductTagDto>>> ListAsync(CancellationToken cancellationToken = default);
        Task<Result<ProductTagDto>> GetAsync(string productTagId, CancellationToken cancellationToken = default);
    }
}
