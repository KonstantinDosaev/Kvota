using Kvota.Interfaces;

namespace Kvota.Models.Products
{
    public class ProductsInStorage: IIdentifiable
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public Guid StorageId { get; set; }
        public virtual Storage? Storage { get; set; }
        public int Quantity { get; set; }
    }
}
