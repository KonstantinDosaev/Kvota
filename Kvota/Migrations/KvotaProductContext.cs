
using Kvota.Models.Products;
using Microsoft.EntityFrameworkCore;

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
            optionsBuilder
                .UseLazyLoadingProxies(); // подключение lazy loading
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<CategoryOption> CategoryOptions { get; set; } = null!;
        public virtual DbSet<GrandCategory> GrandCategories { get; set; } = null!;
        public virtual DbSet<ProductOption> ProductOptions { get; set; } = null!;
    }
}
