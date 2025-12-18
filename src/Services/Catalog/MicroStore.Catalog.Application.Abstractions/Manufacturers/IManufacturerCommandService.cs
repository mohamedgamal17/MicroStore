using MicroStore.BuildingBlocks.Utils.Results;
namespace MicroStore.Catalog.Application.Abstractions.Manufacturers
{
    public interface IManufacturerCommandService
    {
        Task<Result<ManufacturerDto>> CreateAsync(ManufacturerModel model, CancellationToken cancellationToken = default);
        Task<Result<ManufacturerDto>> UpdateAsync(string manufacturerId, ManufacturerModel model, CancellationToken cancellationToken = default);
    }
}
