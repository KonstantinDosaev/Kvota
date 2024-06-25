using Kvota.Constants;
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
        private List<Storage> Storages { get; set; } = new List<Storage>();
        private IEnumerable<Category>? CategoryList { get; set; }
        private Guid StorageId { get; set; }
        public ProductsInStorage ProductsInStorage { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            Storages = (List<Storage>)await StorageRepo.GetAllAsync();
            Product = new Product();
            BrandList = (List<Brand>)await BrandRepo.GetAllAsync();
            CategoryList = await CategoryRepo.GetAllAsync() as List<Category>;
            CategoryList = CategoryList.Where(w => w.Children == null || !w.Children.Any());
            ProductsInStorage.ProductId = Product.Id;
            await InvokeAsync(StateHasChanged);
        }

        private async void SubmitProduct()
        {
            if (ProductsInStorage.ProductId != Guid.Empty || ProductsInStorage.StorageId!=Guid.Empty )
                Product.ProductsInStorage = new List<ProductsInStorage>() { ProductsInStorage };
            Product.Image = Links.DefaultImageProduct;
            Product.DateTimeCreated = DateTime.UtcNow + new TimeSpan(0,3,0,0);
            Product.DateTimeUpdated = Product.DateTimeCreated;
            await ProductRepo.AddAsync(Product);
            NavigationManager!.NavigateTo("/admin/product");
          
        }
    }
}
