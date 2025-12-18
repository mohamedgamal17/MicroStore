using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MicroStore.Ordering.Application.Domain;

namespace MicroStore.Ordering.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class OrderStateEntityTypeConfiguration
         : SagaClassMap<OrderStateEntity>
    {
        protected override void Configure(EntityTypeBuilder<OrderStateEntity> entity, ModelBuilder model)
        {
            entity.Property(x => x.UserId).HasMaxLength(256);

            entity.Property(x => x.OrderNumber).HasMaxLength(265);

            entity.Property(x => x.PaymentId)
                .IsRequired(false)
                .HasMaxLength(256)
                .HasDefaultValue(string.Empty);

            entity.Property(x => x.ShipmentId)
                .HasMaxLength(256)
                .HasDefaultValue(string.Empty);

            entity.OwnsOne(x => x.ShippingAddress, navigationBuilder =>
            {
                navigationBuilder
                  .Property(x => x.CountryCode)
                  .HasDefaultValue(string.Empty)
                  .HasMaxLength(50);

                navigationBuilder
                  .Property(x => x.City)
                  .HasDefaultValue(string.Empty)
                  .HasMaxLength(50);


                navigationBuilder
                .Property(x => x.State)
                .HasDefaultValue(string.Empty)
                .HasMaxLength(50);


                navigationBuilder
                .Property(x => x.PostalCode)
                .HasDefaultValue(string.Empty)
                .HasMaxLength(50);

                navigationBuilder
                 .Property(x => x.Zip)
                 .HasDefaultValue(string.Empty)
                 .HasMaxLength(50);


                navigationBuilder
                  .Property(x => x.AddressLine1)
                  .HasDefaultValue(string.Empty)
                  .HasMaxLength(300);


                navigationBuilder
                  .Property(x => x.AddressLine2)
                  .HasDefaultValue(string.Empty)
                  .HasMaxLength(300);

                navigationBuilder
                  .Property(x => x.Name)
                  .HasDefaultValue(string.Empty)
                  .HasMaxLength(300);

                navigationBuilder
                  .Property(x => x.Phone)
                  .HasDefaultValue(string.Empty)
                  .HasMaxLength(100);
            });

            entity.OwnsOne(x => x.BillingAddress, navigationBuilder =>
            {
                navigationBuilder
                  .Property(x => x.CountryCode)
                  .HasDefaultValue(string.Empty)
                  .HasMaxLength(50);

                navigationBuilder
                  .Property(x => x.City)
                  .HasDefaultValue(string.Empty)
                  .HasMaxLength(50);


                navigationBuilder
                .Property(x => x.State)
                .HasDefaultValue(string.Empty)
                .HasMaxLength(50);


                navigationBuilder
                .Property(x => x.PostalCode)
                .HasDefaultValue(string.Empty)
                .HasMaxLength(50);

                navigationBuilder
                 .Property(x => x.Zip)
                 .HasDefaultValue(string.Empty)
                 .HasMaxLength(50);


                navigationBuilder
                  .Property(x => x.AddressLine1)
                  .HasDefaultValue(string.Empty)
                  .HasMaxLength(300);


                navigationBuilder
                  .Property(x => x.AddressLine2)
                  .HasDefaultValue(string.Empty)
                  .HasMaxLength(300);

                navigationBuilder
                  .Property(x => x.Name)
                  .HasDefaultValue(string.Empty)
                  .HasMaxLength(300);

                navigationBuilder
                  .Property(x => x.Phone)
                  .HasDefaultValue(string.Empty)
                  .HasMaxLength(100);
            });
            entity.Property(x => x.CancellationReason)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasDefaultValue(string.Empty);

            entity.Property(x => x.CurrentState).HasMaxLength(256);
            entity.HasMany(x => x.OrderItems).WithOne();
            entity.HasIndex(x => x.UserId);
            entity.HasIndex(x => x.OrderNumber).IsUnique();
            entity.HasIndex(x => x.SubmissionDate);
            entity.HasIndex(x => x.ShipmentId);
            entity.Navigation(x => x.OrderItems).AutoInclude();
        }
    }
}
