using Kvota.Interfaces;

namespace Kvota.Models.Products
{
    public class Product: IIdentifiable, ISoftDelete
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; } = null!;
        public string? Description { get; set; }
        public string? PartNumber { get; set; }
        public decimal? Price { get; set; }
        public decimal? SalePrice { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public DateTime? DateTimeUpdated { get; set; }
        public DateOnly? DateDelivery { get; set; }
        public int? DayToDelivery { get; set; }
        public virtual Brand? Brand { get; set; }
        public Guid? BrandId { get; set; }
        public virtual Category? Category { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual ICollection<ProductOption>? ProductOption { get; set; }
        public virtual List<Storage>? Storage { get; set; }
        public virtual List<ProductsInStorage>? ProductsInStorage { get; set; }


        public virtual ICollection<ApplicationOrderingProducts>? ApplicationOrderingList { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        public string? UpdatedUser { get; set; }
    }
}
