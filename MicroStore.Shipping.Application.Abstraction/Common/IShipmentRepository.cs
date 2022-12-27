using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IShipmentRepository : IRepository<Shipment,Guid>
    {
        Task<Shipment> RetriveShipment(Guid id, CancellationToken cancellationToken = default);

        Task<Shipment> RetriveShipmentByExternalId(string externalShipmentId , CancellationToken cancellationToken = default);

    }
}
