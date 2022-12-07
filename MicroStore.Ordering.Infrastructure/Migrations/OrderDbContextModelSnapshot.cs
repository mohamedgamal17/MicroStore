﻿// <auto-generated />
using System;
using MicroStore.Ordering.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MicroStore.Ordering.Infrastructure.Migrations
{
    [DbContext(typeof(OrderDbContext))]
    partial class OrderDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("ItemName")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid?>("OrderStateEntityCorrelationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProductImage")
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("OrderStateEntityCorrelationId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItemEntity");
                });

            modelBuilder.Entity("MicroStore.Ordering.Application.StateMachines.OrderStateEntity", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BillingAddressId")
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

                    b.Property<string>("ShipmentSystem")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime?>("ShippedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ShippingAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("ShippingCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SubTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TaxCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserId")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("CorrelationId");

                    b.HasIndex("BillingAddressId");

                    b.HasIndex("OrderNumber")
                        .IsUnique()
                        .HasFilter("[OrderNumber] IS NOT NULL");

                    b.HasIndex("ShipmentId");

                    b.HasIndex("ShippingAddressId");

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
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
