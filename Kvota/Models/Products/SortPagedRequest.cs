using Microsoft.Data.OData.Query;

namespace Kvota.Models.Products
{
    public class SortPagedRequest
    {
        public string? SearchString { get; set; }
        public string? SortLabel { get; set; }
        public OrderByDirection? SortDirection { get; set; }
        public int PageIndex { get; set;}
        public int PageSize { get; set;}
        public string? Chapter { get; set; }
        public Guid? ChapterId { get; set; }
    }
}
