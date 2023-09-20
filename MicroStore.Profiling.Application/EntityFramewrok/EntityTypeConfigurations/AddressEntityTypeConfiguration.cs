using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Profiling.Application.Domain;
namespace MicroStore.Profiling.Application.EntityFramewrok.EntityTypeConfigurations
{
    public class AddressEntityTypeConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x => x.Name).HasMaxLength(400);

            builder.Property(x=> x.CountryCode).HasMaxLength(10);

            builder.Property(x=> x.City).HasMaxLength(50);

            builder.Property(x=> x.State).HasMaxLength(50);

            builder.Property(x=> x.AddressLine1).HasMaxLength(500);

            builder.Property(x=> x.AddressLine2).HasMaxLength(500);

            builder.Property(x=> x.Zip).HasMaxLength(20);

            builder.Property(x=> x.PostalCode).HasMaxLength(20);

        }
    }
}
