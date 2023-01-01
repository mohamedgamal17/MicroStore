using Microsoft.EntityFrameworkCore;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Domain.Entities;
using System.Reflection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
namespace MicroStore.Shipping.PluginInMemoryTest.EntityFramework
{
    [ConnectionStringName("in_memory_db")]
    public class ShippingInMemoryDbContext : AbpDbContext<ShippingInMemoryDbContext>, IShippingDbContext
    {

        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShippingSystem> ShippingSystems { get; set; }

        public ShippingInMemoryDbContext(DbContextOptions<ShippingInMemoryDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
