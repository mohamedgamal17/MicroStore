using Microsoft.EntityFrameworkCore;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MicroStore.Shipping.Infrastructure.EntityFramework
{
    public class ShipmentRepository : EfCoreRepository<ShippingDbContext, Shipment, Guid>, IShipmentRepository
    {
        public ShipmentRepository(IDbContextProvider<ShippingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<Shipment?> RetriveShipment(Guid id, CancellationToken cancellationToken)
        {
            var query = await GetQueryableAsync();

            return await query.Include(x => x.Items)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        }

        public Task<Shipment> RetriveShipmentByExternalId(string externalShipmentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
