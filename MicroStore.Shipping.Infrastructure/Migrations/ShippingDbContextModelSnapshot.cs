﻿// <auto-generated />
using System;
using MicroStore.Shipping.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.EntityFrameworkCore;

#nullable disable

namespace MicroStore.Shipping.Infrastructure.Migrations
{
    [DbContext(typeof(ShippingDbContext))]
    partial class ShippingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("_Abp_DatabaseProvider", EfCoreDatabaseProvider.SqlServer)
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MicroStore.Shipping.Domain.Entities.SettingsEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderKey")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("ProviderKey")
                        .IsUnique();

                    b.ToTable("SettingsEntity");
                });

            modelBuilder.Entity("MicroStore.Shipping.Domain.Entities.Shipment", b =>
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

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasMaxLength(265)
                        .HasColumnType("nvarchar(265)");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ShipmentExternalId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(265)
                        .HasColumnType("nvarchar(265)")
                        .HasDefaultValue("");

                    b.Property<string>("ShipmentLabelExternalId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(265)
                        .HasColumnType("nvarchar(265)")
                        .HasDefaultValue("");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("SystemName")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasDefaultValue("");

                    b.Property<string>("TrackingNumber")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(265)
                        .HasColumnType("nvarchar(265)")
                        .HasDefaultValue("");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(265)
                        .HasColumnType("nvarchar(265)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.HasIndex("OrderNumber")
                        .IsUnique();

                    b.HasIndex("ShipmentExternalId");

                    b.HasIndex("ShipmentLabelExternalId");

                    b.HasIndex("SystemName");

                    b.HasIndex("TrackingNumber");

                    b.HasIndex("UserId");

                    b.ToTable("Shipments");
                });

            modelBuilder.Entity("MicroStore.Shipping.Domain.Entities.ShipmentItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(265)
                        .HasColumnType("nvarchar(265)");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(265)
                        .HasColumnType("nvarchar(265)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("ShipmentId")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Sku")
                        .IsRequired()
                        .HasMaxLength(265)
                        .HasColumnType("nvarchar(265)");

                    b.Property<string>("Thumbnail")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShipmentId");

                    b.HasIndex("Sku");

                    b.ToTable("ShipmentItem");
                });

            modelBuilder.Entity("MicroStore.Shipping.Domain.Entities.Shipment", b =>
                {
                    b.OwnsOne("MicroStore.Shipping.Domain.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<string>("ShipmentId")
                                .HasColumnType("nvarchar(256)");

                            b1.Property<string>("AddressLine1")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(350)
                                .HasColumnType("nvarchar(350)")
                                .HasDefaultValue("")
                                .HasColumnName("Address_AddressLine1");

                            b1.Property<string>("AddressLine2")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(350)
                                .HasColumnType("nvarchar(350)")
                                .HasDefaultValue("")
                                .HasColumnName("Address_AddressLine2");

                            b1.Property<string>("City")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasDefaultValue("")
                                .HasColumnName("Address_City");

                            b1.Property<string>("CountryCode")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasDefaultValue("")
                                .HasColumnName("Address_CountryCode");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(265)
                                .HasColumnType("nvarchar(265)")
                                .HasDefaultValue("")
                                .HasColumnName("Address_CustomerName");

                            b1.Property<string>("Phone")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(265)
                                .HasColumnType("nvarchar(265)")
                                .HasDefaultValue("")
                                .HasColumnName("Address_CustomerPhone");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasDefaultValue("")
                                .HasColumnName("Address_PostalCode");

                            b1.Property<string>("State")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasDefaultValue("")
                                .HasColumnName("Address_State");

                            b1.Property<string>("Zip")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasDefaultValue("")
                                .HasColumnName("Address_Zip");

                            b1.HasKey("ShipmentId");

                            b1.ToTable("Shipments");

                            b1.WithOwner()
                                .HasForeignKey("ShipmentId");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("MicroStore.Shipping.Domain.Entities.ShipmentItem", b =>
                {
                    b.HasOne("MicroStore.Shipping.Domain.Entities.Shipment", null)
                        .WithMany("Items")
                        .HasForeignKey("ShipmentId");

                    b.OwnsOne("MicroStore.Shipping.Domain.ValueObjects.Dimension", "Dimension", b1 =>
                        {
                            b1.Property<string>("ShipmentItemId")
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

                            b1.HasKey("ShipmentItemId");

                            b1.ToTable("ShipmentItem");

                            b1.WithOwner()
                                .HasForeignKey("ShipmentItemId");
                        });

                    b.OwnsOne("MicroStore.Shipping.Domain.ValueObjects.Weight", "Weight", b1 =>
                        {
                            b1.Property<string>("ShipmentItemId")
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

                            b1.HasKey("ShipmentItemId");

                            b1.ToTable("ShipmentItem");

                            b1.WithOwner()
                                .HasForeignKey("ShipmentItemId");
                        });

                    b.Navigation("Dimension")
                        .IsRequired();

                    b.Navigation("Weight")
                        .IsRequired();
                });

            modelBuilder.Entity("MicroStore.Shipping.Domain.Entities.Shipment", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
