﻿using Microsoft.EntityFrameworkCore;
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

            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(600);

            builder.Property(x => x.Sku).IsRequired().HasMaxLength(256);

            builder.Property(x => x.IsFeatured).HasDefaultValue(default(bool));

            builder.Property(x => x.ShortDescription)
                .HasMaxLength(600)
                .IsRequired(false)
                .HasDefaultValue(string.Empty);

            builder.Property(x => x.LongDescription)
                .HasMaxLength(2500)
                .IsRequired(false)
                .HasDefaultValue(string.Empty);

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
                    .HasDefaultValue(Weight.Empty.Unit);
            });

            builder.OwnsOne(x => x.Dimensions, dimensionNavigationBuilder =>
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


            builder.HasMany(x => x.Categories)
                .WithMany(x => x.Products);

            builder.HasMany(x => x.Manufacturers)
                .WithMany(x => x.Products);

            builder.HasMany(x => x.ProductImages).WithOne().HasForeignKey(x=> x.ProductId);

            builder.HasMany(x => x.Tags).WithMany(x=> x.Products);

            builder.HasMany(x => x.SpecificationAttributes).WithOne().HasForeignKey(x=> x.ProductId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasIndex(x => x.Sku).IsUnique();

            builder.Navigation(x => x.Categories).AutoInclude();

            builder.Navigation(x => x.Manufacturers).AutoInclude();

            builder.Navigation(x => x.ProductImages).AutoInclude();

            builder.Navigation(x => x.Tags).AutoInclude();

            builder.Navigation(x => x.SpecificationAttributes).AutoInclude();

        }
    }
}
