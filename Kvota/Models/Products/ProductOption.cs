using Kvota.Interfaces;

namespace Kvota.Models.Products
{
    public class ProductOption:IIdentifiable
    {
        public Guid Id { get; set; }
        public string Value { get; set; } = null!;
        public Guid ProductId { get; set; }
        public Guid CategoryOptionId { get; set; }

        public virtual Product? Product { get; set; }
        public virtual CategoryOption? CategoryOption { get; set; }

    }
}
