using Microsoft.EntityFrameworkCore;
using MicroStore.Shipping.Application.Abstraction.Configuration;
using MicroStore.Shipping.Domain.Entities;

namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IShippingDbContext
    {
         DbSet<Shipment> Shipments { get; set; }

    }
}
