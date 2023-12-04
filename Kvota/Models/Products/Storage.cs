using Kvota.Interfaces;

namespace Kvota.Models.Products
{
    public class Storage : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual Address? Address { get; set; }
        public Guid AddressId { get; set; }

        public virtual List<Product>? Products { get; set; }
        public virtual List<ProductsInStorage>? ProductsInStorage { get; set; }
    }
}
