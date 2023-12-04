using Kvota.Interfaces;
using Kvota.Models.Products;

namespace Kvota.Pages.Products
{
    partial class CatalogPage
    {
        private IEnumerable<Category>? Categories { get; set; }

        protected override async Task OnInitializedAsync()
        {
            using var scope = serviceScopeFactory.CreateScope();
            Categories = await scope.ServiceProvider.GetService<IRepo<Category>>()!.GetAllAsync();
            Categories = Categories.Where(w => w.ParentId == null);
        }
    }
}
