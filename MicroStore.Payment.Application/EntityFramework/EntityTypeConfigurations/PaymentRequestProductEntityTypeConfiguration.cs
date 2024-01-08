using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Payment.Application.Domain;

namespace MicroStore.Payment.Application.EntityFramework.EntityTypeConfigurations
{
    public class PaymentRequestProductEntityTypeConfiguration : IEntityTypeConfiguration<PaymentRequestProduct>
    {
        public void Configure(EntityTypeBuilder<PaymentRequestProduct> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x => x.ProductId).HasMaxLength(256);

            builder.Property(x => x.Sku).HasMaxLength(256);

            builder.Property(x => x.Name).HasMaxLength(300);

            builder.Property(x => x.Thumbnail)
                .HasMaxLength(500)
                .IsRequired(false)
                .HasDefaultValue(string.Empty);

            builder.HasIndex(x => x.ProductId);

            builder.HasIndex(x => x.Sku);

            builder.HasIndex(x => x.Name);

        }
    }
}
 