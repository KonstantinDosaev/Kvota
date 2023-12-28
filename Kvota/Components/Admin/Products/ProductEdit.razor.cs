using BlazorBootstrap;
using Kvota.Models.Products;
using Kvota.Repositories.Products;
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
        private List<Category>? CategoryList { get; set; }
        private List<Storage> Storages { get; set; } = new List<Storage>();
        private Guid StorageId { get; set; }
        private int Quantity { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Product = await ProductRepo.GetOneAsync(Id);
            BrandList = (List<Brand>)await BrandRepo.GetAllAsync();
            CategoryList = (List<Category>)await CategoryRepo.GetAllAsync();
            CategoryList = CategoryList.Where(w => w.Children == null || !w.Children.Any()).ToList();
            Storages = (List<Storage>)await StorageRepo.GetAllAsync();
         
            if (Product.Storage != null && Product.Storage.Any())
            {
                var pId = Product.Storage.Select(s => s.Id);
                Storages = Storages.Where(w=>!pId.Contains(w.Id)).ToList();
            }
        }

        private async void SubmitProduct()
        {
            if (Product != null)
            {
                Product.DateTimeUpdated = DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);
                await ProductRepo.Update(Product);
            }
        }

        private async void AddImagePatch(string patch)
        {
            if (patch == string.Empty) return;
            if (Product != null)
                Product.Image = patch;
            await ProductRepo.Update(Product);
        }   
        //private async void UpdateStorageQuantity(ProductsInStorage item)
        //{
        //    if (Product!.ProductsInStorage != null && Product.ProductsInStorage.Contains(item))
        //    {
        //        await ProductInStorageRepo.Update(item);
        //    }
        //    else
        //    {
        //        await ProductInStorageRepo.AddAsync(item);
        //    }
        //}
    }
}
