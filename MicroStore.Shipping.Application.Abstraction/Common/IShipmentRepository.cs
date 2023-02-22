using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IShipmentRepository : IRepository<Shipment,string>
    {
        Task<Shipment> RetriveShipment(string id, CancellationToken cancellationToken = default);


    }
}
