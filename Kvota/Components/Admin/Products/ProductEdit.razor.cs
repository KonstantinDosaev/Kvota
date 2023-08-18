using BlazorBootstrap;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;

namespace Kvota.Components.Admin.Products
{
    partial class ProductEdit
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
        
        [Parameter]
        public Guid Id { get; set; }
        public Product? Product { get; set; }
        private List<Brand> BrandList { get; set; } = new List<Brand>();
        private List<GrandCategory>? GrandCategoryList { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            Product = await ProductRepo.GetOneAsync(Id);
            BrandList = (List<Brand>)await BrandRepo.GetAllAsync();
            GrandCategoryList = (List<GrandCategory>)await GrandCategoryRepo.GetAllAsync();
        }

        private async void SubmitProduct()
        {
            if (Product != null)
            {
                Product.DateTimeUpdated = DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);
                await ProductRepo.Update(Product);
            }

           // NavigationManager.NavigateTo("/admin/product");
        }

        private void AddImagePatch(string patch)
        {
            if (patch == string.Empty) return;
            if (Product != null)
                Product.Image = patch;
        }
    }
}
