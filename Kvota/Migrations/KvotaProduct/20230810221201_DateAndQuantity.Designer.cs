﻿// <auto-generated />
using System;
using Kvota.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Kvota.Migrations.KvotaProduct
{
    [DbContext(typeof(KvotaProductContext))]
    [Migration("20230810221201_DateAndQuantity")]
    partial class DateAndQuantity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Kvota.Models.Products.Brand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("Kvota.Models.Products.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid?>("GrandCategoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GrandCategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Kvota.Models.Products.CategoryOption", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Measure")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("CategoryOptions");
                });

            modelBuilder.Entity("Kvota.Models.Products.GrandCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("GrandCategories");
                });

            modelBuilder.Entity("Kvota.Models.Products.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("BrandId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly?>("DateDelivery")
                        .HasColumnType("date");

                    b.Property<DateTime?>("DateTimeCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PartNumber")
                        .HasColumnType("text");

                    b.Property<decimal?>("Price")
                        .HasColumnType("numeric");

                    b.Property<int?>("Quantity")
                        .HasColumnType("integer");

                    b.Property<int?>("QuantityTwo")
                        .HasColumnType("integer");

                    b.Property<decimal?>("SalePrice")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Kvota.Models.Products.ProductOption", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryOptionId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryOptionId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductOptions");
                });

            modelBuilder.Entity("Kvota.Models.Products.Category", b =>
                {
                    b.HasOne("Kvota.Models.Products.GrandCategory", "GrandCategory")
                        .WithMany("Categories")
                        .HasForeignKey("GrandCategoryId");

                    b.Navigation("GrandCategory");
                });

            modelBuilder.Entity("Kvota.Models.Products.CategoryOption", b =>
                {
                    b.HasOne("Kvota.Models.Products.Category", "Category")
                        .WithMany("CategoryOptions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Kvota.Models.Products.Product", b =>
                {
                    b.HasOne("Kvota.Models.Products.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandId");

                    b.HasOne("Kvota.Models.Products.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Brand");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Kvota.Models.Products.ProductOption", b =>
                {
                    b.HasOne("Kvota.Models.Products.CategoryOption", "CategoryOption")
                        .WithMany("ProductOption")
                        .HasForeignKey("CategoryOptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kvota.Models.Products.Product", "Product")
                        .WithMany("ProductOption")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CategoryOption");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Kvota.Models.Products.Brand", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Kvota.Models.Products.Category", b =>
                {
                    b.Navigation("CategoryOptions");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("Kvota.Models.Products.CategoryOption", b =>
                {
                    b.Navigation("ProductOption");
                });

            modelBuilder.Entity("Kvota.Models.Products.GrandCategory", b =>
                {
                    b.Navigation("Categories");
                });

            modelBuilder.Entity("Kvota.Models.Products.Product", b =>
                {
                    b.Navigation("ProductOption");
                });
#pragma warning restore 612, 618
        }
    }
}