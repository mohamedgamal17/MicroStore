using Microsoft.EntityFrameworkCore;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MicroStore.Shipping.Infrastructure.EntityFramework
{
    public class ShipmentRepository : EfCoreRepository<ShippingDbContext, Shipment, string>, IShipmentRepository
    {
        public ShipmentRepository(IDbContextProvider<ShippingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<Shipment?> RetriveShipment(string id, CancellationToken cancellationToken)
        {
            var query = await GetQueryableAsync();

            return await query.Include(x => x.Items)
                .SingleAsync(x => x.Id == id, cancellationToken);

        }

      
    }
}
