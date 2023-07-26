using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;

namespace Kvota.Pages.Admin
{
    partial class ProductsTool
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
        private IEnumerable<Product> Products { get; set; } = default!;
        protected IRepo<Product> ProductService { get; set; } = default!;
        private string jpg = "jpg";
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
