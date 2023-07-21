using Kvota.Interfaces;

namespace Kvota.Models.Products
{
    public class GrandCategory:IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; } = null!;
        public string? Description { get; set; }
        public virtual ICollection<Category>? Categories { get; set; }
    }
}
