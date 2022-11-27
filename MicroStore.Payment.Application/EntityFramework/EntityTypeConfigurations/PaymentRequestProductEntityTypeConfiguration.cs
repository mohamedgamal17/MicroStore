using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Payment.Domain.Shared.Domain;

namespace MicroStore.Payment.Application.EntityFramework.EntityTypeConfigurations
{
    public class PaymentRequestProductEntityTypeConfiguration : IEntityTypeConfiguration<PaymentRequestProduct>
    {
        public void Configure(EntityTypeBuilder<PaymentRequestProduct> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductId).HasMaxLength(500);

            builder.Property(x => x.Sku).HasMaxLength(500);

            builder.Property(x => x.Name).HasMaxLength(500);

            builder.Property(x => x.Image).HasMaxLength(800);

            builder.HasIndex(x => x.ProductId);

            builder.HasIndex(x => x.Sku);

            builder.HasIndex(x => x.Name);

        }
    }
}
 