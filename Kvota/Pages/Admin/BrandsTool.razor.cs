

using BlazorBootstrap;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;

namespace Kvota.Pages.Admin
{
    partial class BrandsTool
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
            
            BrandList = (List<Brand>)await BrandRepos.GetAllAsync();

        }


        private async void DeleteBrand(Guid id)
        {
            try
            {
                await BrandRepos.DeleteAsync(id);
                NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
            }
            catch
            {
                modalError?.ShowAsync();
            }

        }

        private void OnUpdateBrand(Guid id)
        {
            BrandUpdate = BrandRepos.GetOne(id);
            modal?.ShowAsync();
        }

        private void OnAddBrand()
        {
            Brand = new Brand();
            modalAdd?.ShowAsync();

        }
        private async void SubmitBrandAdd()
        {
            await BrandRepos.AddAsync(Brand);
            NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
        async void SubmitBrandUpdate()
        {
            await BrandRepos.Update(BrandUpdate);
            NavigationManager!.NavigateTo(NavigationManager.Uri, forceLoad: true);


        }
    }
}
