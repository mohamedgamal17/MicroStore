using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Payment.Domain;
using Volo.Abp.EntityFrameworkCore.Modeling;
namespace MicroStore.Payment.Application.EntityFramework.EntityTypeConfigurations
{
    public class PaymentRequestEntityTypeConfiguration : IEntityTypeConfiguration<PaymentRequest>
    {
        public void Configure(EntityTypeBuilder<PaymentRequest> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x => x.OrderId).HasMaxLength(256).IsRequired();

            builder.Property(x => x.OrderNumber).HasMaxLength(265).IsRequired();

            builder.Property(x => x.CustomerId).HasMaxLength(265).IsRequired();

            builder.Property(x => x.TransctionId).HasMaxLength(265);

            builder.Property(x => x.Description).HasMaxLength(500);
                
            builder.HasIndex(x => x.OrderId).IsUnique();

            builder.HasIndex(x => x.OrderNumber).IsUnique();

            builder.HasIndex(x => x.CustomerId);

            builder.HasIndex(x => x.TransctionId);

            builder.HasMany(x => x.Items).WithOne();

            builder.ApplyObjectExtensionMappings();
        }
    }
}
