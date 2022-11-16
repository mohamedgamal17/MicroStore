using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Payment.Application.Domain;

namespace MicroStore.Payment.Application.EntityFramework.EntityTypeConfigurations
{
    public class PaymentRequestEntityTypeConfiguration : IEntityTypeConfiguration<PaymentRequest>
    {
        public void Configure(EntityTypeBuilder<PaymentRequest> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OrderId).IsRequired();

            builder.Property(x => x.OrderNumber).HasMaxLength(265).IsRequired();

            builder.Property(x => x.TransctionId).HasMaxLength(265);

            builder.Property(x => x.Amount).IsRequired();

            builder.Property(x => x.FaultAt);

            builder.Property(x => x.FaultReason).HasMaxLength(265);

            builder.HasIndex(x => x.OrderId).IsUnique();

            builder.HasIndex(x => x.OrderNumber).IsUnique();

            builder.HasIndex(x => x.TransctionId);

        }
    }
}
