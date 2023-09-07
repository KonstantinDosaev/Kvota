using System.Text.Json;
using BlazorBootstrap;
using Kvota.Constants;
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
        private void GetModalUpdate(Guid id, string title)
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
            await using var openStream = File.OpenRead($"{Links.RootPath}/{Links.ContactsJson}");
            Contacts = (await JsonSerializer.DeserializeAsync<ContactsModel>(openStream))!;
            await InvokeAsync(StateHasChanged);

        }
    }
}
