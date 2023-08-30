using BlazorBootstrap;
using Kvota.Models.Content;
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
        private ContactsModel Contacts { get; set; } = null!;
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
        private async void OnDropdownShowingAsync()
        {
            if (Contacts != null) return;
            Contacts = await ContactService.GetOneAsync(new Guid("80beea30-3f74-42f3-812b-561cea25ec32"));
            await InvokeAsync(StateHasChanged);

        }
    }
}
