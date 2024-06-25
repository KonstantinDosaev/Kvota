using Kvota.Interfaces;

namespace Kvota.Models.Products
{
    public class Storage : IIdentifiable, ISoftDelete
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual Address? Address { get; set; }
        public Guid AddressId { get; set; }

        public virtual List<Product>? Products { get; set; }
        public virtual List<ProductsInStorage>? ProductsInStorage { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        public string? UpdatedUser { get; set; }
    }
}
