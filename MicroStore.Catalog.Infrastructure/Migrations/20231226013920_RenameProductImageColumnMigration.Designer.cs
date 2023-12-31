﻿// <auto-generated />
using System;
using MicroStore.Catalog.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.EntityFrameworkCore;

#nullable disable

namespace MicroStore.Catalog.Infrastructure.Migrations
{
    [DbContext(typeof(CatalogDbContext))]
    [Migration("20231226013920_RenameProductImageColumnMigration")]
    partial class RenameProductImageColumnMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("_Abp_DatabaseProvider", EfCoreDatabaseProvider.SqlServer)
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.Category", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasColumnName("ConcurrencyStamp");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<string>("Description")
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<string>("ExtraProperties")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ExtraProperties");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.Manufacturer", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasColumnName("ConcurrencyStamp");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<string>("Description")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasDefaultValue("");

                    b.Property<string>("ExtraProperties")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ExtraProperties");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasColumnName("ConcurrencyStamp");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DeleterId");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("DeletionTime");

                    b.Property<string>("ExtraProperties")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ExtraProperties");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("IsDeleted");

                    b.Property<bool>("IsFeatured")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<string>("LongDescription")
                        .HasMaxLength(2500)
                        .HasColumnType("nvarchar(2500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<double>("OldPrice")
                        .HasColumnType("float");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ShortDescription")
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<string>("Sku")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("Sku")
                        .IsUnique();

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.ProductCategory", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductCategory");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.ProductImage", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImage");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.ProductManufacturer", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ManufacturerId")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("ManufacturerId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductManufacturer");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.ProductReview", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DeleterId");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("DeletionTime");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("ReplayText")
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<string>("ReviewText")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("ProductReviews");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.ProductSpecificationAttribute", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("AttributeId")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("OptionId")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.HasIndex("OptionId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductSpecificationAttribute");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.ProductTag", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasMaxLength(650)
                        .HasColumnType("nvarchar(650)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ProductTags");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.SpecificationAttribute", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Description")
                        .HasMaxLength(650)
                        .HasColumnType("nvarchar(650)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("SpecificationAttributes");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.SpecificationAttributeOption", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("SpecificationAttributeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("SpecificationAttributeId");

                    b.ToTable("SpecificationAttributeOption");
                });

            modelBuilder.Entity("ProductProductTag", b =>
                {
                    b.Property<string>("ProductTagsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProductsId")
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("ProductTagsId", "ProductsId");

                    b.HasIndex("ProductsId");

                    b.ToTable("ProductProductTag");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.Product", b =>
                {
                    b.OwnsOne("MicroStore.Catalog.Domain.ValueObjects.Dimension", "Dimensions", b1 =>
                        {
                            b1.Property<string>("ProductId")
                                .HasColumnType("nvarchar(256)");

                            b1.Property<double>("Height")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("float")
                                .HasDefaultValue(0.0)
                                .HasColumnName("Height_Width");

                            b1.Property<double>("Length")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("float")
                                .HasDefaultValue(0.0)
                                .HasColumnName("Dimension_Lenght");

                            b1.Property<int>("Unit")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasDefaultValue(0)
                                .HasColumnName("Dimension_Unit");

                            b1.Property<double>("Width")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("float")
                                .HasDefaultValue(0.0)
                                .HasColumnName("Dimension_Width");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.OwnsOne("MicroStore.Catalog.Domain.ValueObjects.Weight", "Weight", b1 =>
                        {
                            b1.Property<string>("ProductId")
                                .HasColumnType("nvarchar(256)");

                            b1.Property<int>("Unit")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasDefaultValue(0)
                                .HasColumnName("Weight_Unit");

                            b1.Property<double>("Value")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("float")
                                .HasDefaultValue(0.0)
                                .HasColumnName("Weight_Value");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.Navigation("Dimensions")
                        .IsRequired();

                    b.Navigation("Weight")
                        .IsRequired();
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.ProductCategory", b =>
                {
                    b.HasOne("MicroStore.Catalog.Domain.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MicroStore.Catalog.Domain.Entities.Product", null)
                        .WithMany("ProductCategories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.ProductImage", b =>
                {
                    b.HasOne("MicroStore.Catalog.Domain.Entities.Product", null)
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.ProductManufacturer", b =>
                {
                    b.HasOne("MicroStore.Catalog.Domain.Entities.Manufacturer", "Manufacturer")
                        .WithMany()
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MicroStore.Catalog.Domain.Entities.Product", null)
                        .WithMany("ProductManufacturers")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manufacturer");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.ProductReview", b =>
                {
                    b.HasOne("MicroStore.Catalog.Domain.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.ProductSpecificationAttribute", b =>
                {
                    b.HasOne("MicroStore.Catalog.Domain.Entities.SpecificationAttribute", "Attribute")
                        .WithMany()
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MicroStore.Catalog.Domain.Entities.SpecificationAttributeOption", "Option")
                        .WithMany()
                        .HasForeignKey("OptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MicroStore.Catalog.Domain.Entities.Product", null)
                        .WithMany("SpecificationAttributes")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");

                    b.Navigation("Option");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.SpecificationAttributeOption", b =>
                {
                    b.HasOne("MicroStore.Catalog.Domain.Entities.SpecificationAttribute", null)
                        .WithMany("Options")
                        .HasForeignKey("SpecificationAttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProductProductTag", b =>
                {
                    b.HasOne("MicroStore.Catalog.Domain.Entities.ProductTag", null)
                        .WithMany()
                        .HasForeignKey("ProductTagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MicroStore.Catalog.Domain.Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.Product", b =>
                {
                    b.Navigation("ProductCategories");

                    b.Navigation("ProductImages");

                    b.Navigation("ProductManufacturers");

                    b.Navigation("SpecificationAttributes");
                });

            modelBuilder.Entity("MicroStore.Catalog.Domain.Entities.SpecificationAttribute", b =>
                {
                    b.Navigation("Options");
                });
#pragma warning restore 612, 618
        }
    }
}
