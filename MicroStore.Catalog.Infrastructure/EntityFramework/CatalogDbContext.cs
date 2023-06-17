using Microsoft.EntityFrameworkCore;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Domain.Entities;
using System.Reflection;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
namespace MicroStore.Catalog.Infrastructure.EntityFramework
{
    [ConnectionStringName("DefaultConnection")]
    [ExposeServices(new Type[] {  typeof(DbContext) }, IncludeSelf = true, IncludeDefaults = true)]
    public class CatalogDbContext : AbpDbContext<CatalogDbContext>, ICatalogDbContext ,ITransientDependency
    {
        public DbSet<Product> Products { get; set; } 
        public DbSet<Category> Categories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get ; set ; }
        public DbSet<ProductReview> ProductReviews { get ; set ; }
        public DbSet<ProductTag> ProductTags { get ; set ; }
        public DbSet<SpecificationAttribute> SpecificationAttributes { get ; set; }

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
