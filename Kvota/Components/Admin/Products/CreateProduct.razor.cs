using Microsoft.AspNetCore.Components;
using Kvota.Models.Products;

namespace Kvota.Components.Admin.Products
{
    partial class CreateProduct
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        public Product Product { get; set; } = null!;
        private List<Brand> BrandList { get; set; }= new List<Brand>();
        private List<Category> CategoriesList { get; set; } = new List<Category>();
        protected override async Task OnInitializedAsync()
        {
            Product = new Product();
            BrandList = (List<Brand>)await BrandRepo.GetAllAsync();
            CategoriesList = (List<Category>)await CategoryRepo.GetAllAsync();
        }

        private async void SubmitProduct()
        {
            await ProductRepo.AddAsync(Product);
            NavigationManager.NavigateTo("/admin/product");
        }
    }
}
