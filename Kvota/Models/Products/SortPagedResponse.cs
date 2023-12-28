namespace Kvota.Models.Products
{
    public class SortPagedResponse <T>
    {
        public IEnumerable<T>? Items { get; set; }
        public int TotalItems { get; set; }
    }
}
