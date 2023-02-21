using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Inventory.Domain.OrderAggregate;
namespace MicroStore.Inventory.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x => x.OrderNumber).HasMaxLength(256);

            builder.Property(x => x.PaymentId).HasMaxLength(256);

            builder.Property(x => x.UserId).HasMaxLength(256);

            builder.OwnsOne(x => x.ShippingAddress, navigationBuilder =>
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

            builder.OwnsOne(x => x.BillingAddres, navigationBuilder =>
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


            builder.HasIndex(x => x.OrderNumber).IsUnique();

            builder.HasIndex(x => x.PaymentId).IsUnique();

            builder.HasIndex(x => x.UserId);
        }
    }
}
