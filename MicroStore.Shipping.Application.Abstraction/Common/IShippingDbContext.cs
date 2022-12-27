using Microsoft.EntityFrameworkCore;
using MicroStore.Shipping.Domain.Entities;

namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IShippingDbContext
    {
         DbSet<Shipment> Shipments { get; set; }

         DbSet<ShippingSystem> ShippingSystems { get; set; }
    }
}
