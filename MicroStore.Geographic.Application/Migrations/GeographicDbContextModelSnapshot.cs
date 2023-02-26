﻿// <auto-generated />
using MicroStore.Geographic.Application.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.EntityFrameworkCore;

#nullable disable

namespace MicroStore.Geographic.Application.Migrations
{
    [DbContext(typeof(GeographicDbContext))]
    partial class GeographicDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("_Abp_DatabaseProvider", EfCoreDatabaseProvider.SqlServer)
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MicroStore.Geographic.Application.Domain.Country", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<int>("NumericIsoCode")
                        .HasColumnType("int");

                    b.Property<string>("ThreeLetterIsoCode")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("TwoLetterIsoCode")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.HasKey("Id");

                    b.HasIndex("ThreeLetterIsoCode")
                        .IsUnique();

                    b.HasIndex("TwoLetterIsoCode")
                        .IsUnique();

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("MicroStore.Geographic.Application.Domain.StateProvince", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("CountryId")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.HasIndex("Abbreviation");

                    b.HasIndex("CountryId");

                    b.HasIndex("Name");

                    b.ToTable("StateProvinces");
                });

            modelBuilder.Entity("MicroStore.Geographic.Application.Domain.StateProvince", b =>
                {
                    b.HasOne("MicroStore.Geographic.Application.Domain.Country", null)
                        .WithMany("StateProvinces")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MicroStore.Geographic.Application.Domain.Country", b =>
                {
                    b.Navigation("StateProvinces");
                });
#pragma warning restore 612, 618
        }
    }
}
