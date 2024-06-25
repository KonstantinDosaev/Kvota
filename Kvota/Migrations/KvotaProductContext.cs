
using Kvota.Models;
using Kvota.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace Kvota.Migrations
{
    public class KvotaProductContext : DbContext
    {
        public KvotaProductContext(DbContextOptions<KvotaProductContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>()
                .HasQueryFilter(x => x.IsDeleted == false);
            builder.Entity<Storage>()
                .HasQueryFilter(x => x.IsDeleted == false);
            builder.Entity<ApplicationOrderingProducts>()
                .HasQueryFilter(x => x.IsDeleted == false);


            builder.Entity<Category>(entity =>
            {
                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Children)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<Product>()
                .HasOne(u => u.Category)
                .WithMany(c => c.Products).HasForeignKey(f => f.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(builder);

            builder
                .Entity<Storage>()
                .HasMany(c => c.Products)
                .WithMany(s => s.Storage)
                .UsingEntity<ProductsInStorage>(
                    j => j
                        .HasOne(pt => pt.Product)
                        .WithMany(t => t.ProductsInStorage)
                        .HasForeignKey(pt => pt.ProductId),
                    j => j
                        .HasOne(pt => pt.Storage)
                        .WithMany(p => p.ProductsInStorage)
                        .HasForeignKey(pt => pt.StorageId),
                    j =>
                    {
                        j.Property(pt => pt.Quantity).HasDefaultValue(0);
                        j.HasKey(t => new { t.StorageId, t.ProductId });
                        j.ToTable("ProductsInStorages");
                    });
            builder
                .Entity<ApplicationOrderingProducts>()
                .HasMany(c => c.ProductList)
                .WithMany(s => s.ApplicationOrderingList)
                .UsingEntity<ApplicationOrderingProductsProduct>(
                    j => j
                        .HasOne(pt => pt.Product)
                        .WithMany(t => t.ApplicationOrderingProductProduct)
                        .HasForeignKey(pt => pt.ProductId),
                    j => j
                        .HasOne(pt => pt.ApplicationOrdering)
                        .WithMany(p => p.ApplicationOrderingProductProduct)
                        .HasForeignKey(pt => pt.ApplicationOrderingId),
                    j =>
                    {
                        j.Property(pt => pt.Quantity).HasDefaultValue(0);
                        j.HasKey(t => new { t.ApplicationOrderingId, t.ProductId });
                        j.ToTable("ApplicationOrderingProductsProducts");
                    });


        }


        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<CategoryOption> CategoryOptions { get; set; } = null!;
        public virtual DbSet<ProductOption> ProductOptions { get; set; } = null!;
        public DbSet<Storage> Storages { get; set; } = null!;
        public DbSet<ProductsInStorage> ProductsInStorages { get; set; } = null!;
        public DbSet<ApplicationOrderingProducts> ApplicationOrderingProducts { get; set; } = null!;
        public DbSet<ApplicationOrderingProductsProduct> ApplicationOrderingProductsProducts { get; set; } = null!;
    }
}
