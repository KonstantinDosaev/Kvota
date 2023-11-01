using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorBootstrap;
using Kvota.Constants;
using Kvota.Models.Content;


namespace Kvota.Pages.Products
{
    partial class ProductCard
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
        [Parameter]
        public Guid Id { get; set; }
        public Product Product { get; set; } = null!;
        private string? _directoryPath;
        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = null!;
        private Modal? _modalImage;
        private ContactsModel Contacts { get; set; } = null!;

        protected override async Task OnParametersSetAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            Product = await scope.ServiceProvider.GetService<IRepo<Product>>()!.GetOneAsync(Id);
            _directoryPath = $"{Links.RootPath}/{Product.Image}";
            await InvokeAsync(StateHasChanged);
        }


        protected async Task GetModalImage()
        {
           // await JsRuntime.InvokeVoidAsync("unslickSlider");
           await _modalImage?.ShowAsync()!;
        }

        private async void OnModalShown()
        {
            await JsRuntime.InvokeVoidAsync("onBlazorReady");
        }
        private async Task OnHideModalClick()
        {
            await _modalImage?.HideAsync()!;
        }
        private async void OnDropdownShowingAsync()
        {
            if (Contacts != null) return;
            Contacts = await ContactSerialize.DeSerialize($"{Links.RootPath}/{Links.ContactsJson}");
            await InvokeAsync(StateHasChanged);
        }

       
    }
}
