using Kvota.Models.Products;
using System.Text;

namespace Kvota.Interfaces
{
    public interface IMissingProductConverter
    {
        public List<MissingProductInCatalog> ConvertMissingProductStringToList(string joinedString);
        public string? ConvertMissingProductListToString(List<MissingProductInCatalog>? list);
    }
}
