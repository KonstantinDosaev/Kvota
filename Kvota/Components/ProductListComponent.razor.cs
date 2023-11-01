using BlazorBootstrap;
using Kvota.Constants;
using Kvota.Models.Content;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;

namespace Kvota.Components
{
    partial class ProductListComponent
    {
        [Parameter]
        public IEnumerable<Product>? ProductList { get; set; }

        [Parameter]
        public Product? Product { get; set; }
        private Modal? _modalProductCard;
        private Guid IdCurrent { get; set; }
        private ContactsModel Contacts { get; set; } = null!;
        private void GetModalUpdate(Guid id)
        {
            IdCurrent = id;
            _modalProductCard?.ShowAsync();
            
        }
        private async Task OnHideModalClick()
        {
            await _modalProductCard?.HideAsync()!;
        }
        private async void OnDropdownShowingAsync()
        {
            if (Contacts != null) return;
            Contacts = await ContactSerialize.DeSerialize($"{Links.RootPath}/{Links.ContactsJson}");
            await InvokeAsync(StateHasChanged);

        }
    }
}
