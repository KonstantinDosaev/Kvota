using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Kvota.Components
{
    partial class ProductFilterComponent
    {
        private IEnumerable<GrandCategory>? GrandCategoryList { get; set; }
        [Parameter]
        public IEnumerable<Product> Products { get; set; } = null!;

        private static IEnumerable<Product> _filteredList = null!;
        private IEnumerable<Brand>? BrandList { get; set; }
        private string _sortString = "";
        private static IEnumerable<Product>? _pagedList;
        private int _totalPg;
        private int _quantityInPage=2;
        private int _currentPageNumber;

        [Parameter]
        public Guid CategoriesFilterId { get; set; }
        [Parameter]
        public Guid BrandsFilterId { get; set; }

        [Parameter] 
        public string? Category { get; set; } 
        protected string SearchString { get; set; } = string.Empty;

        [Parameter]
        public EventCallback<IEnumerable<Product>> ProductListCallback { get; set; }
        protected override async Task OnInitializedAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            GrandCategoryList = await scope.ServiceProvider.GetService<IRepo<GrandCategory>>()!.GetAllAsync();
            BrandList = Products.Where(w=>w.BrandId!=null).Select(s => s.Brand).Distinct().OrderBy(o => o!.Name)!;
            _currentPageNumber = 1;
            GetList();
            await InvokeAsync(StateHasChanged);
        }

        protected void GetList()
        {
            _filteredList = Products;

            if (CategoriesFilterId != Guid.Empty)
            {
                _filteredList = _filteredList
                    .Where(x => x.CategoryId == CategoriesFilterId)
                    .ToList();
            }
            if (BrandsFilterId != Guid.Empty)
            {
                _filteredList = _filteredList
                    .Where(x => x.BrandId == BrandsFilterId)
                    .ToList();
            }

            if (_sortString != string.Empty)
            {
                _filteredList = _sortString switch
                {
                    "name" => _filteredList.OrderBy(o => o.Name).ToList(),
                    "nameDesc" => _filteredList.OrderByDescending(o => o.Name).ToList(),
                    "dateNews" => _filteredList.OrderByDescending(o => o.DateTimeUpdated).ToList(),
                    "priceMin" => _filteredList.OrderByDescending(o => o.Price).ToList(),
                    "priceMax" => _filteredList.OrderBy(o => o.Price).ToList(),
                    _ => _filteredList
                };
            }

            
            OnPageChanged(1);
        }

        private async void Search()
        {
            if (!string.IsNullOrEmpty(SearchString))
            {
                _pagedList = _filteredList
                    .Where(x => x.Name.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1)
                    .ToList();
                await ProductListCallback.InvokeAsync(_pagedList);
            }
            if (string.IsNullOrEmpty(SearchString))
            {
                ResetSearch();
            }

        }
        private void ResetSearch()
        {
            SearchString = string.Empty;
            OnPageChanged(_currentPageNumber);
        }


        private async void OnPageChanged(int newPageNumber)
        {
            _currentPageNumber = newPageNumber;
            _pagedList = _filteredList.Skip((newPageNumber - 1) * _quantityInPage).Take(_quantityInPage);
            _totalPg = (int)Math.Ceiling((double)_filteredList.Count() / _quantityInPage);
            await ProductListCallback.InvokeAsync(_pagedList);

        }

    }
}

