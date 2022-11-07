#nullable disable
using System.Reflection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Data;
using MicroStore.ShoppingCart.Application.Abstraction;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MicroStore.ShoppingCart.Domain.Entities;

namespace MicroStore.ShoppingCart.Infrastructure.EntityFramework
{
    [ExposeServices(typeof(IBasketDbContext), IncludeDefaults = true, IncludeSelf = true)]
    [ConnectionStringName("DefaultConnection")]
    public class BasketDbContext : AbpDbContext<BasketDbContext>, IBasketDbContext
    {

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Product> Products { get; set; }

        public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }


    }
}
