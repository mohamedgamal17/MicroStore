using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IShipmentSystemResolver
    {
        Task<ResultV2<IShipmentSystemProvider>> Resolve(string systemName, CancellationToken cancellationToken = default);
    }
}
