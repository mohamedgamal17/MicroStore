using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Entities.ElasticSearch;
namespace MicroStore.Catalog.Application.Abstractions.Manufacturers
{
    public interface IManufacturerQueryService
    {
        Task<Result<List<ElasticManufacturer>>> ListAsync(ManufacturerListQueryModel queryParams, CancellationToken cancellationToken = default);
        Task<Result<ElasticManufacturer>> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
