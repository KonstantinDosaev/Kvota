using Kvota.Interfaces;
using Kvota.Models.Products;
using Kvota.Models.UserAuth;

namespace Kvota.Models
{
    public class ApplicationOrderingProducts : IIdentifiable, ISoftDelete
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string UserId { get; set; }
        public string? UserName { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyInn { get; set; }
        public virtual ICollection<Product>? ProductList { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public virtual ICollection<ApplicationOrderingProductsProduct>? ApplicationOrderingProductProduct { get; set; }

        public DateTime? DateTimeCreated { get; set; }
        public bool? InWork { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        public string? UpdatedUser { get; set; }
        public string? TakerUser { get; set; }
        public string? TakerUserId { get; set; }
        public DateTime? DateTimeTake { get; set; }
        public string? MissingProductsInCatalog { get; set; }
    }
}
