using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;
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
        private ContactsModel Contacts { get; set; } = null!;
        private bool _visibleImageSlider;
        protected override async Task OnParametersSetAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            Product = await scope.ServiceProvider.GetService<IRepo<Product>>()!.GetOneAsync(Id);
            if (Product.Image!= null)
            {
                _directoryPath = $"{Links.RootPath}/{Product.Image}";
            }
            
            await InvokeAsync(StateHasChanged);
        }


        protected void GetModalImage()
        {
            _visibleImageSlider = true;
        }

        //private async void OnModalShown()
        //{
        //    await JsRuntime.InvokeVoidAsync("onBlazorReady");
        //}
        private void OnHideModalClick()
        {
            //await _modalImage?.HideAsync()!;
            _visibleImageSlider = false;
        }
        private async void OnDropdownShowingAsync()
        {
            if (Contacts != null) return;
            Contacts = await ContactSerialize.DeSerialize($"{Links.RootPath}/{Links.ContactsJson}");
            await InvokeAsync(StateHasChanged);
        }

       
    }
}
