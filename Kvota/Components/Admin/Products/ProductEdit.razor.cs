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
        public Product Product { get; set; } = null!;
        private List<Brand> BrandList { get; set; } = new List<Brand>();
        private List<GrandCategory>? GrandCategoryList { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            Product =  ProductRepo.GetOne(Id);
            BrandList = (List<Brand>)await BrandRepo.GetAllAsync();
            GrandCategoryList = (List<GrandCategory>)await GrandCategoryRepo.GetAllAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async void SubmitProduct()
        {
            Product.DateTimeUpdated= DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);
            await ProductRepo.Update(Product);
            NavigationManager.NavigateTo("/admin/product");
        }

        private void AddImagePatch(string patch)
        {
            if (patch != string.Empty)
                Product.Image = patch;
        }
    }
}
