using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Catalog.Domain.Entities;

namespace MicroStore.Catalog.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class ProductReviewEntityTypeConfiguration : IEntityTypeConfiguration<ProductReview>
    {
        public void Configure(EntityTypeBuilder<ProductReview> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x => x.Title).HasMaxLength(600);

            builder.Property(x => x.ReviewText).HasMaxLength(600);

            builder.Property(x => x.ReplayText)
                .IsRequired(false)
                .HasMaxLength(600)
                .HasDefaultValue(string.Empty);

            builder.Property(x => x.UserId).HasMaxLength(256);

            builder.Property(x => x.ProductId).HasMaxLength(256);

            builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);

            builder.HasIndex(x => x.UserId);
        }
    }
}
