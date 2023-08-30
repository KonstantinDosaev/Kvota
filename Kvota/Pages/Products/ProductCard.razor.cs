using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorBootstrap;
using Kvota.Models.Content;


namespace Kvota.Pages.Products
{
    partial class ProductCard
    {
        [Parameter]
        public Guid Id { get; set; }
        public Product Product { get; set; } = null!;
        private List<Brand> BrandList { get; set; } = new List<Brand>();
        private List<GrandCategory>? GrandCategoryList { get; set; }
        private string? _directoryPath;
        private int _fullQuantity;
        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = null!;
        private Modal? _modalImage;
        private ContactsModel Contacts { get; set; } = null!;

        protected override async Task OnParametersSetAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            Product = await scope.ServiceProvider.GetService<IRepo<Product>>()!.GetOneAsync(Id);
            //BrandList = (List<Brand>)await BrandRepo.GetAllAsync();
            //GrandCategoryList = (List<GrandCategory>)await GrandCategoryRepo.GetAllAsync();
            _directoryPath = $"{env.WebRootPath}\\{Product.Image}";
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
            await _modalImage?.HideAsync();
        }
        private async void OnDropdownShowingAsync()
        {
            if (Contacts != null) return;
            Contacts = await ContactService.GetOneAsync(new Guid("80beea30-3f74-42f3-812b-561cea25ec32"));
            await InvokeAsync(StateHasChanged);

        }
    }
}
