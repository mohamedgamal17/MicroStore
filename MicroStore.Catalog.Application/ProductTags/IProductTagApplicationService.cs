using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.ProductTags;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.Application.Services;

namespace MicroStore.Catalog.Application.ProductTags
{
    public interface IProductTagApplicationService  : IApplicationService
    {
        Task<Result<ProductTagDto>> CreateAsync(ProductTagModel model , CancellationToken cancellationToken = default);
        Task<Result<ProductTagDto>> UpdateAsync(string productTagId, ProductTagModel productTag , CancellationToken cancellationToken = default);
        Task<Result<Unit>> RemoveAsync(string productTagId, CancellationToken cancellationToken = default);
        Task<Result<List<ElasticProductTag>>> ListAsync(CancellationToken cancellationToken = default);
        Task<Result<ElasticProductTag>> GetAsync(string productTagId, CancellationToken cancellationToken = default);
    }
}
