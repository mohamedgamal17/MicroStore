using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IShipmentSystemResolver
    {
        Task<Result<IShipmentSystemProvider>> Resolve(string systemName, CancellationToken cancellationToken = default);
    }
}
