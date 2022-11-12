#nullable disable
using Microsoft.EntityFrameworkCore;
using MicroStore.Inventory.Application.Abstractions.Common;
using MicroStore.Inventory.Domain.ProductAggregate;
using System.Reflection;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
namespace MicroStore.Inventory.Infrastructure.EntityFramework
{
    [ConnectionStringName("DefaultConnection")]
    [ExposeServices(typeof(IInventoyDbContext))]
    public class InventoyDbContext : AbpDbContext<InventoyDbContext> , IInventoyDbContext
    {
        public DbSet<Product> Products { get; set; }
        public InventoyDbContext(DbContextOptions<InventoyDbContext> options) 
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
