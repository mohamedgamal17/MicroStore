using Microsoft.EntityFrameworkCore;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MicroStore.Shipping.PluginInMemoryTest.EntityFramework
{
    public class ShipmentRepository : EfCoreRepository<ShippingInMemoryDbContext, Shipment, Guid>, IShipmentRepository
    {

        public ShipmentRepository(IDbContextProvider<ShippingInMemoryDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<Shipment?> RetriveShipment(Guid id, CancellationToken cancellationToken)
        {
            var query = await GetQueryableAsync();

            return await query.Include(x => x.Items)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        }

        public async Task<Shipment?> RetriveShipmentByExternalId(string externalShipmentId, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query.Include(x => x.Items)
                .SingleOrDefaultAsync(x => x.ShipmentExternalId == externalShipmentId, cancellationToken);
        }

    }
}
