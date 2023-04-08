
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models;

namespace MicroStore.Catalog.Application.Manufacturers
{
    public interface IManufacturerCommandService
    {
        Task<Result<ManufacturerDto>> CreateAsync(ManufacturerModel model, CancellationToken cancellationToken = default);
        Task<Result<ManufacturerDto>> UpdateAsync(string manufacturerId , ManufacturerModel model, CancellationToken cancellationToken = default);
    }
}
