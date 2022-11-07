using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.ShoppingCart.Domain.Entities;

namespace MicroStore.ShoppingCart.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class BasketEntityTypeConfiguration : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.HasKey(x => x.Id);


            builder.Property(x => x.UserId);

            builder.HasIndex(x => x.UserId)
                .IsUnique(true);

            builder.Metadata?.FindNavigation(nameof(Basket.LineItems))?
                 .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(x => x.LineItems).AutoInclude();
        }
    }
}
