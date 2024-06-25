using Kvota.Constants;
using Kvota.Interfaces;
using Kvota.Models.Products;
using Kvota.Repositories.Products;
using Microsoft.AspNetCore.Components;
using MudBlazor;


namespace Kvota.Pages.Products
{
    partial class ProductList
    {
        private IEnumerable<Product> Products { get; set; } = new List<Product>();
        public List<ProductInOrder>? ProductInOrderList { get; set; }

        private static List<Product>? _pagedList;
        private static IEnumerable<Product>? _filteredList;
        [Parameter]
        public string? Groups { get; set; }
        [Parameter] 
        public string? GroupName { get; set; }

        [Parameter]
        public Guid GroupId { get; set; }
        [Parameter]
        public Guid? BrandId { get; set; }
        private int _quantityInPage = 10;
        private int _currentPageCount = 10;
        private int _totalItems;
        private int _paqeIndex = 0;
        private string? _title;
        private string? _searchString;
        private string _sortString = SortLabelsProduct.NameField;
        private SortDirection _sortDirection = SortDirection.Descending;
        private FilterProductTuple? _filterProductTuple;
        private bool _visibleProductFilter;
        protected override async Task OnInitializedAsync()
        {
        
            //await InitList();
        }
        protected override async Task OnParametersSetAsync()
        {
           await InitList();
        }
        private async Task<List<Product>> ServerReload()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var productService = scope.ServiceProvider.GetService<IRepo<Product>>();
            var response = await ((ProductRepo)productService!).GetBySortPagedSearchChapterAsync(new SortPagedRequest<FilterProductTuple>()
            {
                PageIndex = _paqeIndex,
                PageSize = _quantityInPage,
                SearchString = _searchString,
                SortLabel = _sortString,
                Chapter = Groups,
                ChapterId = GroupId,
                SortDirection = _sortDirection,
                FilterTuple = _filterProductTuple

            });
            //  pagedData = response.Items;
            if (response.Items != null && Groups == GroupNames.GroupCategory && response.Items.Any())
            {
                _title = response.Items.FirstOrDefault()!.Category?.Name;
            }
            else
            {
                _title = null;
            }

            _totalItems = response.TotalItems;
            return (response.Items ?? Array.Empty<Product>()).ToList();
        }

        private async Task InitList()
        {
            await GetOrderList();
           _paqeIndex = 0;
           if (BrandId != Guid.Empty && BrandId != null)
           {
               _filterProductTuple ??= new FilterProductTuple();
               _filterProductTuple.BrandIdList ??= new List<Guid>();
               _filterProductTuple.BrandIdList.Add((Guid)BrandId);
           }
            _pagedList = await ServerReload();
           
        }
       
        private async Task LoadMore()
        {
            if (_pagedList!.Count == _totalItems)
                return;
            _paqeIndex+= 1;
            _pagedList!.AddRange(await ServerReload());
        }

        protected async Task GetOrderList()
        {
            try
            {
                var response = await LocalStorage.GetAsync<List<ProductInOrder>>("OrderList");
                if (response.Value != null) ProductInOrderList = response.Value;
            }
            catch
            {
                await LocalStorage.DeleteAsync("OrderList");
            }
           
        }
        private async Task OnSortDirection()
        {
            _paqeIndex = 0;
            _sortDirection = _sortDirection == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
           _pagedList = await ServerReload();
        }
        private async Task OnSorted()
        {
            _paqeIndex = 0;
            _pagedList = await ServerReload();
        }
        private async Task OnSearch(string text)
        {
            _paqeIndex = 0;
            _searchString = text;
            _pagedList = await ServerReload();
        }
        private async Task OnFilter(FilterProductTuple filterProductTuple)
        {
            _paqeIndex = 0;
            _filterProductTuple = filterProductTuple;
            _pagedList = await ServerReload();
        }
    }
}
