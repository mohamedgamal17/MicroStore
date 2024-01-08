﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Payment.Application.Domain;
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

            builder.Property(x => x.UserId).HasMaxLength(265).IsRequired();

            builder.Property(x => x.TransctionId).HasMaxLength(265)
                .IsRequired(false)
                .HasDefaultValue(string.Empty);

            builder.Property(x => x.PaymentGateway).HasMaxLength(265)
                .IsRequired(false)
                .HasDefaultValue(string.Empty);

            builder.Property(x => x.Description).HasMaxLength(500)
                .IsRequired(false)
                .HasDefaultValue(string.Empty);


            builder.Property(x => x.CapturedAt)
                .HasDefaultValue(DateTime.MinValue);


            builder.Property(x => x.FaultAt)
                .HasDefaultValue(DateTime.MinValue);

            builder.Property(x => x.RefundedAt)
                .HasDefaultValue(DateTime.MinValue);

            builder.HasIndex(x => x.OrderId).IsUnique();

            builder.HasIndex(x => x.OrderNumber).IsUnique();

            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => x.TransctionId);

            builder.HasMany(x => x.Items).WithOne();

            builder.ApplyObjectExtensionMappings();
        }
    }
}
