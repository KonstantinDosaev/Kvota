using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;

namespace Kvota.Pages.Products
{
    partial class CatalogPage
    {
        private IEnumerable<GrandCategory>? GrandCategories { get; set; } 
        private IEnumerable<Category>? Categories { get; set; }

        protected override async Task OnInitializedAsync()
        {
            using var scope = serviceScopeFactory.CreateScope();
            GrandCategories = await scope.ServiceProvider.GetService<IRepo<GrandCategory>>()!.GetAllAsync();
            await InvokeAsync(StateHasChanged);
        }
    }
}
