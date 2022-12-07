using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MicroStore.Ordering.Application.StateMachines;
namespace MicroStore.Ordering.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class OrderStateEntityTypeConfiguration
         : SagaClassMap<OrderStateEntity>
    {

        protected override void Configure(EntityTypeBuilder<OrderStateEntity> entity, ModelBuilder model)
        {
            entity.Property(x => x.UserId).HasMaxLength(256);
            entity.Property(x => x.OrderNumber).HasMaxLength(265);
            entity.Property(x => x.PaymentId).IsRequired(false).HasMaxLength(256);
            entity.Property(x => x.ShipmentId).HasMaxLength(256);
            entity.Property(x => x.ShipmentSystem).HasMaxLength(256);
            entity.Property(x => x.CancellationReason).HasMaxLength(500);
            entity.Property(x => x.CurrentState).HasMaxLength(256);
            entity.HasMany(x => x.OrderItems).WithOne();
            entity.HasIndex(x => x.UserId);
            entity.HasIndex(x => x.OrderNumber).IsUnique();
            entity.HasIndex(x => x.ShippingAddressId);
            entity.HasIndex(x => x.BillingAddressId);
            entity.HasIndex(x => x.ShipmentId);
            entity.Navigation(x => x.OrderItems).AutoInclude();
        }
    }
}
