using System.Text;

namespace Kvota.Models.Products
{
    public class MissingProductInCatalog
    {
        public string PartNumber { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public int Quantity { get; set; }

        
    }
}
