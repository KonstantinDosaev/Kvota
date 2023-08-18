using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;


namespace Kvota.Pages.Products
{
    partial class ProductList
    {
        private IEnumerable<Product> Products { get; set; } = new List<Product>();

        private static List<Product>? _pagedList;
        private static IEnumerable<Product>? _filteredList;
        [Parameter]
        public Guid CategoriesId { get; set; }
        [Parameter]
        public Guid BrandsId { get; set; }

        [Parameter] 
        public string? Category { get; set; }
        private int _quantityInPage = 10;
        private int _currentPageCount = 10;
        protected override async Task OnInitializedAsync()
        {
            using var scope = serviceScopeFactory.CreateScope();
            if (Category != null)
            {
                Products = await scope.ServiceProvider.GetService<IRepo<Product>>()!.GetAllByIdAsync(CategoriesId);
            }
            else
            {
                Products = await scope.ServiceProvider.GetService<IRepo<Product>>()!.GetAllAsync();
            }
            _pagedList = new List<Product>();
            _filteredList = Products;

           LoadMore(0);
            
           
            
        }
        private void LoadMore(int newPageNumber)
        {
            _currentPageCount = newPageNumber;
            _pagedList.AddRange((_filteredList.Skip((_currentPageCount)).Take(_quantityInPage)).ToList());

        }

        protected void GetFilterList(IEnumerable<Product> filteredList)
        {
            _filteredList = filteredList;
            _pagedList = new List<Product>();
            LoadMore(0);

        }

    }
}
