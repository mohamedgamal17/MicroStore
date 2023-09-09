using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Models.Manufacturers;
using MicroStore.Catalog.Entities.ElasticSearch;
namespace MicroStore.Catalog.Application.Manufacturers
{
    public interface IManufacturerQueryService
    {
        Task<Result<List<ElasticManufacturer>>> ListAsync(ManufacturerListQueryModel queryParams, CancellationToken cancellationToken = default);
        Task<Result<ElasticManufacturer>> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
