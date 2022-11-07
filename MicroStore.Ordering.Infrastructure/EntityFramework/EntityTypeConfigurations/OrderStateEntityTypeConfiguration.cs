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
            entity.Property(x => x.TransactionId).IsRequired(false).HasMaxLength(256);
            entity.Property(x => x.ShippmentId).HasMaxLength(256);
            entity.Property(x => x.CancelledBy).HasMaxLength(256);
            entity.Property(x => x.RejectedBy).HasMaxLength(256);
            entity.Property(x => x.RejectionReason).HasMaxLength(500);
            entity.Property(x => x.FaultReason).HasMaxLength(500);
            entity.Property(x => x.CancellationReason).HasMaxLength(500);

            entity.HasMany(x => x.OrderItems).WithOne();

            entity.HasIndex(x => x.UserId);
            entity.HasIndex(x => x.OrderNumber).IsUnique();
            entity.HasIndex(x => x.ShippingAddressId);
            entity.HasIndex(x => x.BillingAddressId);
            entity.HasIndex(x => x.ShippmentId);
            entity.HasIndex(x => x.CancelledBy);
            entity.HasIndex(x => x.RejectedBy);


            entity.Navigation(x => x.OrderItems).AutoInclude();
        }
    }
}
