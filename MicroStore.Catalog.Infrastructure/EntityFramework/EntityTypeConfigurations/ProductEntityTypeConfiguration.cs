using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Catalog.Domain.Const;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Domain.ValueObjects;
namespace MicroStore.Catalog.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    internal class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(600);

            builder.Property(x => x.Sku).IsRequired().HasMaxLength(256);

            builder.Property(x => x.Thumbnail).IsRequired().HasDefaultValue(string.Empty).HasMaxLength(600);

            builder.Property(x => x.ShortDescription).HasMaxLength(600);

            builder.Property(x => x.LongDescription).HasMaxLength(2500);

            builder.Property(x => x.Price);

            builder.Property(x => x.OldPrice);

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
                    .HasMaxLength(WeightConst.UnitLenght)
                    .HasDefaultValue(Weight.Empty.Unit);
            });

            builder.OwnsOne(x => x.Height, heighNavigationBuilder =>
            {
                heighNavigationBuilder
                    .Property(x => x.Value)
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName(HeightColumnConst.Value)
                    .HasDefaultValue(Dimension.Empty.Value);

                heighNavigationBuilder
                    .Property(x => x.Unit)
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName(HeightColumnConst.Unit)
                    .HasMaxLength(DimensionConst.UnitLenght)
                    .HasDefaultValue(Dimension.Empty.Unit);
            });

            builder.OwnsOne(x => x.Length, lenghtNavigationBuilder =>
            {

                lenghtNavigationBuilder
                    .Property(x => x.Value)
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName(LenghtColumnConst.Value)
                    .HasDefaultValue(Dimension.Empty.Value);

                lenghtNavigationBuilder
                    .Property(x => x.Unit)
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName(LenghtColumnConst.Unit)
                    .HasMaxLength(DimensionConst.UnitLenght)
                    .HasDefaultValue(Dimension.Empty.Unit);

            });

            builder.OwnsOne(x => x.Width, widthNavigationBuilder =>
            {   
                widthNavigationBuilder
                    .Property(x => x.Value)
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName(WidthColumnConst.Value)
                    .HasDefaultValue(Dimension.Empty.Value);

                widthNavigationBuilder
                    .Property(x => x.Unit)
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName(WidthColumnConst.Unit)
                    .HasMaxLength(DimensionConst.UnitLenght)
                    .HasDefaultValue(Dimension.Empty.Unit);

            });


            builder.HasMany(x => x.ProductCategories).WithOne();

            builder.HasMany(x => x.ProductImages).WithOne();

            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasIndex(x => x.Sku).IsUnique();

            builder.Navigation(x => x.ProductCategories).AutoInclude();

            builder.Navigation(x => x.ProductImages).AutoInclude();

        }
    }
}
