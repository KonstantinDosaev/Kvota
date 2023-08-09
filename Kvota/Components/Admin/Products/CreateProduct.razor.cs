using Microsoft.AspNetCore.Components;
using Kvota.Models.Products;
using Newtonsoft.Json.Linq;
using Kvota.Interfaces;
using Kvota.Repositories.Products;
using static System.Formats.Asn1.AsnWriter;

namespace Kvota.Components.Admin.Products
{
    partial class CreateProduct
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        public Product Product { get; set; } = null!;
        private List<Brand> BrandList { get; set; }= new List<Brand>();
        private IEnumerable<GrandCategory>? GrandCategoryList { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Product = new Product();
            BrandList = (List<Brand>)await BrandRepo.GetAllAsync();
            GrandCategoryList = (List<GrandCategory>)await GrandCategoryRepo.GetAllAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async void SubmitProduct()
        {
            Product.Image = "image\\products\\default.jpg";
            Product.DateTimeCreated = DateTime.UtcNow + new TimeSpan(0,3,0,0);
            Product.DateTimeUpdated = Product.DateTimeCreated;
            await ProductRepo.AddAsync(Product);
            NavigationManager!.NavigateTo("/admin/product");
        }
    }
}
