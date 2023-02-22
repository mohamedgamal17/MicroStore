using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.Shipping.Domain.ValueObjects;
namespace MicroStore.Shipping.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class ShipmentEntityTypeConfigurationBuilder : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x => x.OrderId)
                .IsRequired()
                .HasMaxLength(265);

            builder.Property(x => x.OrderNumber).IsRequired().HasMaxLength(256);

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasMaxLength(265);

            builder.Property(x => x.ShipmentExternalId)
                .HasDefaultValue(string.Empty)
                .HasMaxLength(265);

            builder.Property(x => x.TrackingNumber)
                .HasDefaultValue(string.Empty)
                .HasMaxLength(265);

            builder.Property(x => x.ShipmentLabelExternalId)
                .HasDefaultValue(string.Empty)
                .HasMaxLength(265);

            builder.Property(x => x.SystemName)
                .HasDefaultValue(string.Empty)
                .HasMaxLength(256);

            builder.OwnsOne(x => x.Address, addressNavigationBuilder =>
            {

                addressNavigationBuilder
                    .Property(x => x.CountryCode)
                    .HasColumnName(AddressConst.CountryCode)
                    .HasDefaultValue(Address.Empty.CountryCode)
                    .HasMaxLength(AddressConst.CountryCodeMaxLength);

                addressNavigationBuilder
                    .Property(x => x.City)
                    .HasColumnName(AddressConst.City)
                    .HasDefaultValue(Address.Empty.City)
                    .HasMaxLength(AddressConst.CityMaxLength);


                addressNavigationBuilder
                  .Property(x => x.State)
                  .HasColumnName(AddressConst.State)
                  .HasDefaultValue(Address.Empty.State)
                  .HasMaxLength(AddressConst.StateMaxLength);


                addressNavigationBuilder
                  .Property(x => x.PostalCode)
                  .HasColumnName(AddressConst.PostalCode)
                  .HasDefaultValue(Address.Empty.PostalCode)
                  .HasMaxLength(AddressConst.PostalCodeMaxLength);

                addressNavigationBuilder
                 .Property(x => x.Zip)
                 .HasColumnName(AddressConst.Zip)
                 .HasDefaultValue(Address.Empty.Zip)
                 .HasMaxLength(AddressConst.ZipMaxLength);


                addressNavigationBuilder
                 .Property(x => x.AddressLine1)
                 .HasColumnName(AddressConst.AddressLine1)
                 .HasDefaultValue(Address.Empty.AddressLine1)
                 .HasMaxLength(AddressConst.AddressLineMaxLenght);


                addressNavigationBuilder
                 .Property(x => x.AddressLine2)
                 .HasColumnName(AddressConst.AddressLine2)
                 .HasDefaultValue(Address.Empty.AddressLine2)
                 .HasMaxLength(AddressConst.AddressLineMaxLenght);

                addressNavigationBuilder
                 .Property(x => x.Name)
                 .HasColumnName(AddressConst.Name)
                 .HasDefaultValue(Address.Empty.Name)
                 .HasMaxLength(AddressConst.NameMaxLength);

                addressNavigationBuilder
                  .Property(x => x.Phone)
                  .HasColumnName(AddressConst.Phone)
                  .HasDefaultValue(Address.Empty.Phone)
                  .HasMaxLength(AddressConst.PhoneMaxLength);
            });

            builder.HasIndex(x => x.OrderId).IsUnique(true);

            builder.HasIndex(x => x.OrderNumber).IsUnique(true);

            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => x.ShipmentExternalId);

            builder.HasIndex(x => x.TrackingNumber);

            builder.HasIndex(x => x.ShipmentLabelExternalId);

            builder.HasIndex(x => x.SystemName);
        }
    }
}
