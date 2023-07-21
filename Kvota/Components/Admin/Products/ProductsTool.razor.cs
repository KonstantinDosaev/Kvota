using Kvota.Interfaces;
using Kvota.Models.Content;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;


namespace Kvota.Components.Admin.Products
{
    partial class ProductsTool
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
        private IEnumerable<Product> Products { get; set; } = default!;
        protected IRepo<Product> ProductService { get; set; } = default!;
        protected override async Task OnInitializedAsync()
        {
            using var scope = serviceScopeFactory.CreateScope();
            ProductService = scope.ServiceProvider.GetService<IRepo<Product>>()!;
            Products = await ProductService.GetAllAsync();
            await InvokeAsync(StateHasChanged);
        }
        private async void DeleteProduct(Guid id)
        {
            await ProductRepo.DeleteAsync(id);
            NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
    }
}
