using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models;
using MicroStore.Catalog.Application.Models.Manufacturers;

namespace MicroStore.Catalog.Application.Manufacturers
{
    public interface IManufacturerQueryService
    {
        Task<Result<List<ManufacturerDto>>> ListAsync(ManufacturerListQueryModel queryParams, CancellationToken cancellationToken = default);
        Task<Result<ManufacturerDto>> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
