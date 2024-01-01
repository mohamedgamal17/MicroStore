using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.Manufacturers;
namespace MicroStore.Catalog.Application.Manufacturers
{
    public interface IManufacturerCommandService
    {
        Task<Result<ManufacturerDto>> CreateAsync(ManufacturerModel model, CancellationToken cancellationToken = default);
        Task<Result<ManufacturerDto>> UpdateAsync(string manufacturerId , ManufacturerModel model, CancellationToken cancellationToken = default);
    }
}
