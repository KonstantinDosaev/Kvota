using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace Kvota.Pages.Admin
{
    partial class ProductsTool
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
        private IEnumerable<Product> Products { get; set; } = default!;
        protected IRepo<Product> ProductService { get; set; } = default!;
        private string _sortString = "";
        private static IEnumerable<Product>? _pagedList;
        private int _totalPg;
        private int _quantityInPage = 2;
        private int _currentPageNumber = 1;

        [Parameter]
        public Guid CategoriesFilterId { get; set; }
        [Parameter]
        public Guid BrandsFilterId { get; set; }
        protected string SearchString { get; set; } = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            ProductService = scope.ServiceProvider.GetService<IRepo<Product>>()!;
            Products = await ProductService.GetAllAsync();
            GetList(Products);
            await InvokeAsync(StateHasChanged);
        }
        private async void DeleteProduct(Guid id,string pathImage)
        {
            var path = $"{Env.WebRootPath}\\{pathImage}";
            var fileInf = new FileInfo(path);
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }
            await ProductRepo.DeleteAsync(id);
            NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
        protected async void GetList(IEnumerable<Product> filteredList)
        {

            _pagedList = filteredList;
            await InvokeAsync(StateHasChanged);
        }

       
    }
}
