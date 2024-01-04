using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.Application.Services;
namespace MicroStore.Catalog.Application.Abstractions.ProductTags
{
    public interface IProductTagApplicationService : IApplicationService
    {
        Task<Result<ProductTagDto>> CreateAsync(ProductTagModel model, CancellationToken cancellationToken = default);
        Task<Result<ProductTagDto>> UpdateAsync(string productTagId, ProductTagModel productTag, CancellationToken cancellationToken = default);
        Task<Result<Unit>> RemoveAsync(string productTagId, CancellationToken cancellationToken = default);
        Task<Result<List<ElasticTag>>> ListAsync(CancellationToken cancellationToken = default);
        Task<Result<ElasticTag>> GetAsync(string productTagId, CancellationToken cancellationToken = default);
    }
}
