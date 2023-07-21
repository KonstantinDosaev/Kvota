using BlazorBootstrap;
using Kvota.Data;
using Kvota.Interfaces;
using Kvota.Models.Products;
using Kvota.Repositories.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;

namespace Kvota.Components.Admin.Products
{
    partial class BrandTool
    {
        private Modal? modal;
        private Modal? modalAdd;
        private Modal? modalError;
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        public Brand Brand { get; set; } = null!; 
        public Brand BrandUpdate { get; set; } = null!;
        private List<Brand> BrandList { get; set; } = new List<Brand>();

        protected override async Task OnInitializedAsync()
        {
           
            BrandList = (List<Brand>)await BrandRepo.GetAllAsync();
          
        }

        
        private async void DeleteBrand(Guid id)
        {
            try
            {
                await BrandRepo.DeleteAsync(id);
                NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
            }
            catch
            {
                modalError?.ShowAsync();
            }
           
        }
        
        private  void OnUpdateBrand(Guid id)
        {
            BrandUpdate = BrandRepo.GetOne(id);
            modal?.ShowAsync();
        }
        
        private void OnAddBrand()
        {
            Brand = new Brand();
            modalAdd?.ShowAsync();
            
        }
        private async void SubmitBrandAdd()
        {
            await BrandRepo.AddAsync(Brand);
            NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
        async void SubmitBrandUpdate()
        {
            await BrandRepo.Update(BrandUpdate);
            NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);


        }
    }
}
