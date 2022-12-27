using Microsoft.EntityFrameworkCore;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Domain.Entities;
using System.Reflection;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace MicroStore.Shipping.Infrastructure.EntityFramework
{
    [ConnectionStringName("DefaultConnection")]
    [ExposeServices(typeof(IShippingDbContext), IncludeDefaults = true, IncludeSelf = true)]
    public class ShippingDbContext : AbpDbContext<ShippingDbContext>, IShippingDbContext
    {
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShippingSystem> ShippingSystems { get; set; }
        public ShippingDbContext(DbContextOptions<ShippingDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
