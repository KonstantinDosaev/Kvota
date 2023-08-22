using Kvota.Interfaces;
using Kvota.Models.Products;
using Kvota.Repositories.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Xml.Linq;
using Microsoft.JSInterop;
using NuGet.Protocol.Plugins;
using BlazorBootstrap;

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
        private string[] _dirFile;
        private int _fullQuantity;
        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = null!;

        private bool _runSlick;
        private Modal? _modalImage;

        protected override async Task OnParametersSetAsync()
        //protected override async Task OnInitializedAsync()
        {
            
            using var scope = ServiceScopeFactory.CreateScope();
            Product = await scope.ServiceProvider.GetService<IRepo<Product>>()!.GetOneAsync(Id);
            //BrandList = (List<Brand>)await BrandRepo.GetAllAsync();
            //GrandCategoryList = (List<GrandCategory>)await GrandCategoryRepo.GetAllAsync();

            _directoryPath = $"{env.WebRootPath}\\{Product.Image}";
            //_dirFile = Directory.GetFiles(_directoryPath);
           
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
    }
}
