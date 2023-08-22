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
        private Modal? _modalProductCard;
        private Guid IdCurrent { get; set; }
        private async void GetModalUpdate(Guid id, string title)
        {
            IdCurrent = id;
            // await InvokeAsync(StateHasChanged);
            _modalProductCard?.ShowAsync();
            
        }
        private async Task OnHideModalClick()
        {
            await _modalProductCard?.HideAsync();
        }
    }
}
