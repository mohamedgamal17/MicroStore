using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MicroStore.ShoppingCart.Domain.Entities;

namespace MicroStore.ShoppingCart.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    internal class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name);

            builder.Property(x => x.Sku).HasMaxLength(256);

            builder.Property(x => x.Price).IsRequired();

        }
    }
}
