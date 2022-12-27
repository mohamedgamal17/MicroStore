﻿// <auto-generated />
using System;
using MicroStore.Payment.Application.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.EntityFrameworkCore;

#nullable disable

namespace MicroStore.Payment.Application.Migrations
{
    [DbContext(typeof(PaymentDbContext))]
    [Migration("20221223020931_PaymentSystemMigration")]
    partial class PaymentSystemMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("_Abp_DatabaseProvider", EfCoreDatabaseProvider.SqlServer)
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MicroStore.Payment.Domain.PaymentRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CapturedAt")
                        .HasColumnType("datetime2");

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

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasMaxLength(265)
                        .HasColumnType("nvarchar(265)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ExtraProperties")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ExtraProperties");

                    b.Property<DateTime?>("FaultAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasMaxLength(265)
                        .HasColumnType("nvarchar(265)");

                    b.Property<string>("PaymentGateway")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefundedAt")
                        .HasColumnType("datetime2");

                    b.Property<double>("ShippingCost")
                        .HasColumnType("float");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<double>("SubTotal")
                        .HasColumnType("float");

                    b.Property<double>("TaxCost")
                        .HasColumnType("float");

                    b.Property<double>("TotalCost")
                        .HasColumnType("float");

                    b.Property<string>("TransctionId")
                        .HasMaxLength(265)
                        .HasColumnType("nvarchar(265)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.HasIndex("OrderNumber")
                        .IsUnique();

                    b.HasIndex("TransctionId");

                    b.ToTable("PaymentRequests");
                });

            modelBuilder.Entity("MicroStore.Payment.Domain.PaymentRequestProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<Guid?>("PaymentRequestId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Sku")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Thumbnail")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("PaymentRequestId");

                    b.HasIndex("ProductId");

                    b.HasIndex("Sku");

                    b.ToTable("PaymentRequestProduct");
                });

            modelBuilder.Entity("MicroStore.Payment.Domain.PaymentSystem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("PaymentSystems");
                });

            modelBuilder.Entity("MicroStore.Payment.Domain.PaymentRequestProduct", b =>
                {
                    b.HasOne("MicroStore.Payment.Domain.PaymentRequest", null)
                        .WithMany("Items")
                        .HasForeignKey("PaymentRequestId");
                });

            modelBuilder.Entity("MicroStore.Payment.Domain.PaymentRequest", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
