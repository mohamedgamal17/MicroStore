#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
using MicroStore.Geographic.Application.Domain;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
namespace MicroStore.Geographic.Application.EntityFramework
{
    [ConnectionStringName("DefaultConnection")]
    [ExposeServices(typeof(DbContext),IncludeDefaults = true, IncludeSelf = true)]
    public class GeographicDbContext : AbpDbContext<GeographicDbContext> , ITransientDependency
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<StateProvince> StateProvinces { get; set; }
        public GeographicDbContext(DbContextOptions<GeographicDbContext> options) 
            : base(options)
        {

        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(b =>
            {
                b.HasKey(x => x.Id);

                b.Property(x => x.Id).HasMaxLength(256);

                b.Property(x => x.Name).HasMaxLength(300);

                b.Property(x => x.TwoLetterIsoCode).HasMaxLength(2);

                b.Property(x => x.ThreeLetterIsoCode).HasMaxLength(3);


                b.HasIndex(x => x.TwoLetterIsoCode).IsUnique();

                b.HasIndex(x => x.ThreeLetterIsoCode).IsUnique();

                b.HasMany(x => x.StateProvinces).WithOne().HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Cascade);

                b.Navigation(x => x.StateProvinces).AutoInclude();
            });


            modelBuilder.Entity<StateProvince>(b =>
            {
                b.HasKey(x => x.Id);

                b.Property(x => x.Id).HasMaxLength(256);

                b.Property(x => x.Name).HasMaxLength(300);

                b.Property(x => x.Abbreviation).HasMaxLength(300);

                b.HasIndex(x => x.Name);

                b.HasIndex(x => x.Abbreviation);
            });
            base.OnModelCreating(modelBuilder);

         
                
        }
    }
}
