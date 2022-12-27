﻿// <auto-generated />
using System;
using MicroStore.Ordering.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MicroStore.Ordering.Infrastructure.Migrations
{
    [DbContext(typeof(OrderDbContext))]
    [Migration("20221222221920_OrderAddressMigration")]
    partial class OrderAddressMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MicroStore.Ordering.Application.StateMachines.OrderItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ExternalProductId")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<Guid?>("OrderStateEntityCorrelationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Sku")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Thumbnail")
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ExternalProductId");

                    b.HasIndex("OrderStateEntityCorrelationId");

                    b.HasIndex("Sku");

                    b.ToTable("OrderItemEntity");
                });

            modelBuilder.Entity("MicroStore.Ordering.Application.StateMachines.OrderStateEntity", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CancellationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CancellationReason")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("CurrentState")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("OrderNumber")
                        .HasMaxLength(265)
                        .HasColumnType("nvarchar(265)");

                    b.Property<string>("PaymentId")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ShipmentId")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime?>("ShippedDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("ShippingCost")
                        .HasColumnType("float");

                    b.Property<double>("SubTotal")
                        .HasColumnType("float");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("TaxCost")
                        .HasColumnType("float");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.Property<string>("UserId")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("CorrelationId");

                    b.HasIndex("OrderNumber")
                        .IsUnique()
                        .HasFilter("[OrderNumber] IS NOT NULL");

                    b.HasIndex("ShipmentId");

                    b.HasIndex("UserId");

                    b.ToTable("OrderStateEntity");
                });

            modelBuilder.Entity("MicroStore.Ordering.Application.StateMachines.OrderItemEntity", b =>
                {
                    b.HasOne("MicroStore.Ordering.Application.StateMachines.OrderStateEntity", null)
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderStateEntityCorrelationId");
                });

            modelBuilder.Entity("MicroStore.Ordering.Application.StateMachines.OrderStateEntity", b =>
                {
                    b.OwnsOne("MicroStore.Ordering.Application.StateMachines.Address", "BillingAddress", b1 =>
                        {
                            b1.Property<Guid>("OrderStateEntityCorrelationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("AddressLine1")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(300)
                                .HasColumnType("nvarchar(300)")
                                .HasDefaultValue("");

                            b1.Property<string>("AddressLine2")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(300)
                                .HasColumnType("nvarchar(300)")
                                .HasDefaultValue("");

                            b1.Property<string>("City")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasDefaultValue("");

                            b1.Property<string>("CountryCode")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasDefaultValue("");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(300)
                                .HasColumnType("nvarchar(300)")
                                .HasDefaultValue("");

                            b1.Property<string>("Phone")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasDefaultValue("");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasDefaultValue("");

                            b1.Property<string>("State")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasDefaultValue("");

                            b1.Property<string>("Zip")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasDefaultValue("");

                            b1.HasKey("OrderStateEntityCorrelationId");

                            b1.ToTable("OrderStateEntity");

                            b1.WithOwner()
                                .HasForeignKey("OrderStateEntityCorrelationId");
                        });

                    b.OwnsOne("MicroStore.Ordering.Application.StateMachines.Address", "ShippingAddress", b1 =>
                        {
                            b1.Property<Guid>("OrderStateEntityCorrelationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("AddressLine1")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(300)
                                .HasColumnType("nvarchar(300)")
                                .HasDefaultValue("");

                            b1.Property<string>("AddressLine2")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(300)
                                .HasColumnType("nvarchar(300)")
                                .HasDefaultValue("");

                            b1.Property<string>("City")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasDefaultValue("");

                            b1.Property<string>("CountryCode")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasDefaultValue("");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(300)
                                .HasColumnType("nvarchar(300)")
                                .HasDefaultValue("");

                            b1.Property<string>("Phone")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasDefaultValue("");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasDefaultValue("");

                            b1.Property<string>("State")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasDefaultValue("");

                            b1.Property<string>("Zip")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasDefaultValue("");

                            b1.HasKey("OrderStateEntityCorrelationId");

                            b1.ToTable("OrderStateEntity");

                            b1.WithOwner()
                                .HasForeignKey("OrderStateEntityCorrelationId");
                        });

                    b.Navigation("BillingAddress");

                    b.Navigation("ShippingAddress");
                });

            modelBuilder.Entity("MicroStore.Ordering.Application.StateMachines.OrderStateEntity", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
