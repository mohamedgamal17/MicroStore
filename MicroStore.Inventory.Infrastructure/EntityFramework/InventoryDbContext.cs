#nullable disable
using Microsoft.EntityFrameworkCore;
using MicroStore.Inventory.Application.Common;
using MicroStore.Inventory.Domain.OrderAggregate;
using MicroStore.Inventory.Domain.ProductAggregate;
using System.Reflection;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
namespace MicroStore.Inventory.Infrastructure.EntityFramework
{
    [ConnectionStringName("DefaultConnection")]
    [ExposeServices(typeof(DbContext), typeof(IInventoyDbContext), IncludeSelf = true,  IncludeDefaults = true)]
    public class InventoryDbContext : AbpDbContext<InventoryDbContext> , IInventoyDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) 
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
