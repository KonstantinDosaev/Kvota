﻿// <auto-generated />
using System;
using Kvota.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Kvota.Migrations.KvotaProduct
{
    [DbContext(typeof(KvotaProductContext))]
    partial class KvotaProductContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ApplicationOrderingProductsProduct", b =>
                {
                    b.Property<Guid>("ApplicationOrderingListId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductListId")
                        .HasColumnType("uuid");

                    b.HasKey("ApplicationOrderingListId", "ProductListId");

                    b.HasIndex("ProductListId");

                    b.ToTable("ApplicationOrderingProductsProduct");
                });

            modelBuilder.Entity("Kvota.Models.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Home")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Placement")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalCode")
                        .HasColumnType("text");

                    b.Property<string>("Region")
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("Kvota.Models.ApplicationOrderingProducts", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CompanyInn")
                        .HasColumnType("text");

                    b.Property<string>("CompanyName")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DateTimeCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("InWork")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsFullDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UpdatedUser")
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ApplicationOrderingProducts");
                });

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

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

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

                    b.Property<DateTime?>("DateTimeUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("DayToDelivery")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsFullDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PartNumber")
                        .HasColumnType("text");

                    b.Property<decimal?>("Price")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("SalePrice")
                        .HasColumnType("numeric");

                    b.Property<string>("UpdatedUser")
                        .HasColumnType("text");

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

            modelBuilder.Entity("Kvota.Models.Products.ProductsInStorage", b =>
                {
                    b.Property<Guid?>("StorageId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("StorageId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductsInStorages", (string)null);
                });

            modelBuilder.Entity("Kvota.Models.Products.Storage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DateTimeUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsFullDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UpdatedUser")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("Storages");
                });

            modelBuilder.Entity("ApplicationOrderingProductsProduct", b =>
                {
                    b.HasOne("Kvota.Models.ApplicationOrderingProducts", null)
                        .WithMany()
                        .HasForeignKey("ApplicationOrderingListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kvota.Models.Products.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Kvota.Models.Products.Category", b =>
                {
                    b.HasOne("Kvota.Models.Products.Category", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Parent");
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
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

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

            modelBuilder.Entity("Kvota.Models.Products.ProductsInStorage", b =>
                {
                    b.HasOne("Kvota.Models.Products.Product", "Product")
                        .WithMany("ProductsInStorage")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kvota.Models.Products.Storage", "Storage")
                        .WithMany("ProductsInStorage")
                        .HasForeignKey("StorageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Storage");
                });

            modelBuilder.Entity("Kvota.Models.Products.Storage", b =>
                {
                    b.HasOne("Kvota.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Kvota.Models.Products.Brand", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Kvota.Models.Products.Category", b =>
                {
                    b.Navigation("CategoryOptions");

                    b.Navigation("Children");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("Kvota.Models.Products.CategoryOption", b =>
                {
                    b.Navigation("ProductOption");
                });

            modelBuilder.Entity("Kvota.Models.Products.Product", b =>
                {
                    b.Navigation("ProductOption");

                    b.Navigation("ProductsInStorage");
                });

            modelBuilder.Entity("Kvota.Models.Products.Storage", b =>
                {
                    b.Navigation("ProductsInStorage");
                });
#pragma warning restore 612, 618
        }
    }
}
