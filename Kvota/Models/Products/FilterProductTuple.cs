namespace Kvota.Models.Products
{
    public class FilterProductTuple
    {
        public Guid? CategoryId { get; set; }
        public List<Guid>? BrandIdList { get; set; }
        public List<Guid>? ProductOptions { get; set; }
        public List<Guid>? AllProductsToGuid { get; set; }
    }
}
