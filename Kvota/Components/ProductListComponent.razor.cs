using BlazorBootstrap;
using Kvota.Models.Products;
using Kvota.Repositories.Products;
using Microsoft.AspNetCore.Components;

namespace Kvota.Components
{
    partial class ProductListComponent
    {
        [Parameter]
        public IEnumerable<Product>? ProductList { get; set; }
        [Parameter]
        public IEnumerable<Brand>? BrandList { get; set; }
        private Modal? _modalProduct;
        private Guid IdCurrent { get; set; }
        private async void GetModalUpdate(Guid id, string title)
        {
            IdCurrent = id;
           // await InvokeAsync(StateHasChanged);
            _modalProduct?.ShowAsync();
            
        }
    }
}
