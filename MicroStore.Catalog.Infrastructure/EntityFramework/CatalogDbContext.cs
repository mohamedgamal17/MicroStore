
using Microsoft.EntityFrameworkCore;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Domain.Entities;
using System.Reflection;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace MicroStore.Catalog.Infrastructure.EntityFramework
{
    [ConnectionStringName("DefaultConnection")]
    [ExposeServices(typeof(ICatalogDbContext), IncludeSelf = true)]
    public class CatalogDbContext : AbpDbContext<CatalogDbContext>, ICatalogDbContext
    {

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<ProductCategory> ProductCategories { get; set; } = null!;


        public CatalogDbContext(DbContextOptions<CatalogDbContext> dbContextOptions)
        : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        }


    }
}
