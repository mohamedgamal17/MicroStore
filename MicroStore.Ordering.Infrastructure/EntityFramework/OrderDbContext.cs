using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using MicroStore.Ordering.Application.Abstractions.Common;
using MicroStore.Ordering.Infrastructure.EntityFramework.EntityTypeConfigurations;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Ordering.Infrastructure.EntityFramework
{
    public class OrderDbContext : SagaDbContext, IOrderDbContext, ITransientDependency
    {
        public OrderDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TEntity> Query<TEntity>() where TEntity : class => Set<TEntity>();


        protected override IEnumerable<ISagaClassMap> Configurations { get { yield return new OrderStateEntityTypeConfiguration(); } }


    }
}
