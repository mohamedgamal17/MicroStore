using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Profiling.Application.Domain;

namespace MicroStore.Profiling.Application.EntityFramewrok.EntityTypeConfigurations
{
    public class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.Property(x=> x.Id).HasMaxLength(256);

            builder.Property(x=> x.UserId).HasMaxLength(256);

            builder.Property(x=> x.FirstName).HasMaxLength(256);

            builder.Property(x=> x.LastName).HasMaxLength(256);

            builder.Property(x => x.Avatar)
                .HasMaxLength(800)
                .IsRequired(false)
                .HasDefaultValue(string.Empty);

            builder.OwnsOne(x => x.Phone, phoneNavigationBuilder =>
            {

                phoneNavigationBuilder.Property(x => x.Number)
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasMaxLength(50);
            });


            builder.Property(x=> x.BirthDate).HasColumnType("date");

            builder.HasMany(x => x.Addresses).WithOne();

            builder.HasIndex(x => x.UserId).IsUnique();

            builder.Navigation(x => x.Addresses).AutoInclude(true);
        }
    }
}
