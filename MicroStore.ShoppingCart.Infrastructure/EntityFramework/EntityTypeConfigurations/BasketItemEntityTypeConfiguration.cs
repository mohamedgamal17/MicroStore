using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.ShoppingCart.Domain.Entities;

namespace MicroStore.ShoppingCart.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    internal class BasketItemEntityTypeConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();


            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.HasOne(x => x.Product)
                .WithMany();


            builder.Navigation(x => x.Product).AutoInclude();
        }
    }
}
