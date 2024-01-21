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
        public string? Groups { get; set; }
        [Parameter] 
        public string? GroupName { get; set; }

        [Parameter]
        public Guid GroupId { get; set; }
        [Parameter]
        public Guid? BrandId { get; set; }
        private int _quantityInPage = 10;
        private int _currentPageCount = 10;
        private string? Title;
        protected override async Task OnInitializedAsync()
        {
           //await InitList();
        }
        protected override async Task OnParametersSetAsync()
        {
           await InitList();
        }
        private async Task InitList()
        {
            using var scope = serviceScopeFactory.CreateScope();
            if (Groups != null)
            {
                Products = await scope.ServiceProvider.GetService<IRepo<Product>>()!.GetAllByIdAsync(GroupId, Groups);
                if (BrandId!=null)
                {
                    Products = Products.Where(w => w.BrandId == BrandId);
                }
            }
            else
            {
                Products = await scope.ServiceProvider.GetService<IRepo<Product>>()!.GetAllAsync();
            }
            _pagedList = new List<Product>();
            _filteredList = Products;
            var tempCategoryNameList = _filteredList.Where(w=>w.Category!=null).Select(s => s.Category!.Name).Distinct().ToList();
         
            Title = tempCategoryNameList.Count()==1 ? tempCategoryNameList.SingleOrDefault() : null;
            LoadMore(0);
        }
        private void LoadMore(int newPageNumber)
        {
            _currentPageCount = newPageNumber;
            _pagedList!.AddRange((_filteredList!.Skip((_currentPageCount)).Take(_quantityInPage)).ToList());
        }

        protected void GetFilterList(IEnumerable<Product> filteredList)
        {
            _filteredList = filteredList;
            _pagedList = new List<Product>();
            LoadMore(0);
            this.StateHasChanged();
        }

    }
}
