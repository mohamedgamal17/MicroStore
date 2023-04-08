using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;

namespace MicroStore.Catalog.Application.Manufacturers
{
    public interface IManufacturerQueryService
    {
        Task<Result<List<ManufacturerDto>>> ListAsync(SortingQueryParams queryParams, CancellationToken cancellationToken = default);
        Task<Result<ManufacturerDto>> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
