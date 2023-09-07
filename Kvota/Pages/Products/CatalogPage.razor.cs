using Kvota.Interfaces;
using Kvota.Models.Products;

namespace Kvota.Pages.Products
{
    partial class CatalogPage
    {
        private IEnumerable<GrandCategory>? GrandCategories { get; set; }

        protected override async Task OnInitializedAsync()
        {
            using var scope = serviceScopeFactory.CreateScope();
            GrandCategories = await scope.ServiceProvider.GetService<IRepo<GrandCategory>>()!.GetAllAsync();
        }
    }
}
