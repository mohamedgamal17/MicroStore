using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.Shipping.Domain.ValueObjects;
namespace MicroStore.Shipping.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class ShipmentItemEntityTypeConfiguration : IEntityTypeConfiguration<ShipmentItem>
    {
        public void Configure(EntityTypeBuilder<ShipmentItem> builder)
        {
            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(265);

            builder.Property(x => x.Sku)
                .IsRequired()
                .HasMaxLength(265);

            builder.Property(x => x.ProductId)
                .IsRequired()
                .HasMaxLength(265);

            builder.Property(x => x.Thumbnail)
                .IsRequired(false)
                .HasMaxLength(600)
                .HasDefaultValue(string.Empty);

            builder.OwnsOne(x => x.Weight, weightNavigationBuilder =>
            {
                weightNavigationBuilder
                    .Property(x => x.Value)
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName(WeightConst.Value)
                    .HasDefaultValue(Weight.Empty.Value);

                weightNavigationBuilder
                    .Property(x => x.Unit)
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName(WeightConst.Unit)
                    .HasDefaultValue(Weight.Empty.Unit);
            });

            builder.OwnsOne(x => x.Dimension, dimensionNavigationBuilder =>
            {
                dimensionNavigationBuilder
                    .Property(x => x.Length)
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName(DimensionConst.Lenght)
                    .HasDefaultValue(Dimension.Empty.Length);

                dimensionNavigationBuilder
                    .Property(x => x.Width)
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName(DimensionConst.Width)
                    .HasDefaultValue(Dimension.Empty.Width);


                dimensionNavigationBuilder
                    .Property(x => x.Height)
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName(DimensionConst.Height)
                    .HasDefaultValue(Dimension.Empty.Height);


                dimensionNavigationBuilder
                    .Property(x => x.Unit)
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName(DimensionConst.Unit)
                    .HasDefaultValue(Dimension.Empty.Unit);
            });

            builder.HasIndex(x => x.Name);

            builder.HasIndex(x => x.Sku);

            builder.HasIndex(x => x.ProductId);
        }
    }
}
