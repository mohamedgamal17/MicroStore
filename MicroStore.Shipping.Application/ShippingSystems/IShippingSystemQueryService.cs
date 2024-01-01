using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Shipping.Application.ShippingSystems
{
    public interface IShippingSystemQueryService : IApplicationService
    {
        Task<Result<List<ShippingSystemDto>>> ListAsync(CancellationToken cancellationToken = default);
    }
}
