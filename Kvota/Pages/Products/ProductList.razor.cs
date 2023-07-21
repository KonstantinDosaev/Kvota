using Kvota.Interfaces;
using Kvota.Models.Content;
using Kvota.Models.Products;

namespace Kvota.Pages.Products
{
    partial class ProductList
    {
        private List<Product> Products { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            using var scope = serviceScopeFactory.CreateScope();
            Products = (List<Product>)await scope.ServiceProvider.GetService<IRepo<Product>>()!.GetAllAsync();

            await InvokeAsync(StateHasChanged);
        }
    }
}
