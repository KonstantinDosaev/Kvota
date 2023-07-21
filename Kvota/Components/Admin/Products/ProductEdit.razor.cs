using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;

namespace Kvota.Components.Admin.Products
{
    partial class ProductEdit
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Parameter]
        public Guid Id { get; set; }
        public Product Product { get; set; } = null!;
        private List<Brand> BrandList { get; set; } = new List<Brand>();
        private List<Category> CategoriesList { get; set; } = new List<Category>();
        protected override async Task OnInitializedAsync()
        {
            Product =  ProductRepo.GetOne(Id);
            BrandList = (List<Brand>)await BrandRepo.GetAllAsync();
            CategoriesList = (List<Category>)await CategoryRepo.GetAllAsync();
        }

        private async void SubmitProduct()
        {
            await ProductRepo.Update(Product);
            NavigationManager.NavigateTo("/admin/product");
        }
    }
}
