using Kvota.Interfaces;
using Kvota.Models.Products;
using Kvota.Repositories.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Xml.Linq;
using NuGet.Protocol.Plugins;

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
        protected override async Task OnParametersSetAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            Product = await scope.ServiceProvider.GetService<IRepo<Product>>()!.GetOneAsync(Id);
            //BrandList = (List<Brand>)await BrandRepo.GetAllAsync();
            //GrandCategoryList = (List<GrandCategory>)await GrandCategoryRepo.GetAllAsync();
            _directoryPath = $"{env.WebRootPath}\\{Product.Image}";
            _dirFile = Directory.GetFiles(_directoryPath);
            await InvokeAsync(StateHasChanged);
        }

    }
}
