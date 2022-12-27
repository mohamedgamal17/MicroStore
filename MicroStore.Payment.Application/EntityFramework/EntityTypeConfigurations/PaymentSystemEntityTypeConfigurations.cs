using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Payment.Domain;
namespace MicroStore.Payment.Application.EntityFramework.EntityTypeConfigurations
{
    public class PaymentSystemEntityTypeConfigurations : IEntityTypeConfiguration<PaymentSystem>
    {
        public void Configure(EntityTypeBuilder<PaymentSystem> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(256).IsRequired();

            builder.Property(x => x.DisplayName).HasMaxLength(256).IsRequired();

            builder.Property(x => x.Image).HasMaxLength(500);

            builder.Property(x => x.IsEnabled);

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
